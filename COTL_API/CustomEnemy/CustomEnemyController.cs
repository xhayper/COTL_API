using System.Collections;
using MMBiomeGeneration;
using Spine.Unity;
using Spine.Unity.Examples;
using UnityEngine;

namespace COTL_API.CustomEnemy;

//this custom enemy controller class is a MELEE enemy class. it is an AI that will
//chase the player and attack them when in range.
//if you need ranged attacks or multiple attack choices, extend this class
//and override the functions to make your own attacks.
public class CustomEnemyController : UnitObject, IAttackResilient
{
    public int NewPositionDistance = 3;
    public float MaintainTargetDistance = 4.5f;
    public float MoveCloserDistance = 4f;
    public float AttackWithinRange = 4f;
    public float CircleCastRadius = 0.5f;
    public float CircleCastOffset = 1f;
    public float TeleportDelayTarget = 1f;
    public float AttackDelay;
    public float KnockbackModifier = 1f;
    public float Angle;
    public float RepathTimer;
    public float TeleportDelay;
    public float MaxAttackDelay;
    public float Damage { get; set; } = 1f;
    public static float signPostParryWindow = 0.2f;
    public static float attackParryWindow = 0.15f;

    public bool DoubleAttack = true;
    public bool ChargeAndAttack = true;
    public bool requireLineOfSite = true;

    public string AttackVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_attack";
    public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/death";
    public string EnragedDeathVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_death_vo";
    public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/gethit";
    public string EnragedGetHitVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_gethit_vo";
    public string DrawbackSwordSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_warning";
    public string attackSoundPath = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_attack";
    public string onHitSoundPath = "event:/enemy/impact_normal";
    public string TransformIntoEnragedVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_transform_vo";
    public string TransformIntoEnragedSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_transform";
    public string EnragedChargeAttackVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_charge_vo";
    public string EnragedChargeAttackSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_charge";
    public string InvincibleOnHitSFX = "event:/dlc/combat/invulnerable_impact";

    public bool ShowDebug;
    public bool FollowPlayer { get; set; }
    public bool canBeParried;

    public Vector2 MaxAttackDelayRandomRange = new(4f, 6f);
    public Vector2 AttackDelayRandomRange = new(0.5f, 2f);
    public Vector3 Force;
    public Vector3 TargetPosition;

    public static List<CustomEnemyController> customEnemyControllers = [];
    public List<Vector3> Points = [];
    public List<Vector3> PointsLink = [];
    public List<Vector3> EndPoints = [];
    public List<Vector3> EndPointsLink = [];

    public ColliderEvents damageColliderEvents;
    public SkeletonAnimation Spine;
    public SimpleSpineFlash SimpleSpineFlash;
    public GameObject TargetObject;
    public SkeletonGhost ghost;
    public State MyState;
    public Health EnemyHealth;

    //TODO: this is the AI for the enemy, to do things like attacking, phase changes, priorities, etc

    //basic code for a melee enemy that attacks and chases player on sight.
    //override this class functions to make your own attacks
    public override void Awake()
    {
        base.Awake();
        if (BiomeGenerator.Instance != null)
            GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
        BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(BiomeGenerator_OnBiomeChangeRoom);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(BiomeGenerator_OnBiomeChangeRoom);
    }

    public override void OnEnable()
    {
        SeperateObject = true;
        base.OnEnable();
        customEnemyControllers.Add(this);
        if (damageColliderEvents != null)
        {
            damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(OnDamageTriggerEnter);
            damageColliderEvents.SetActive(false);
        }
        StartCoroutine(WaitForTarget());
        rb.simulated = true;
    }

    public override void OnDisable()
    {
        CleanupEventInstances();

        health.invincible = false;
        health.IsDeflecting = false;
        SimpleSpineFlash.FlashWhite(false);

        base.OnDisable();

        customEnemyControllers.Remove(this);

        if (damageColliderEvents != null)
            damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(OnDamageTriggerEnter);

        ClearPaths();
        StopAllCoroutines();
        DisableForces = false;
        GetComponent<ShowHPBar>().ForceUpdate();
    }

    public override void OnDieEarly(
        GameObject Attacker,
        Vector3 AttackLocation,
        Health Victim,
        Health.AttackTypes AttackType,
        Health.AttackFlags AttackFlags)
    {
        if (health.HP <= 0.0 && !AttackFlags.HasFlag(Health.AttackFlags.ForceKill))
        {
            //this allows a second life kind of interaction, such as enemy enraging etc.
        }
        else
            base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    }

    public override void OnHitEarly(
        GameObject Attacker,
        Vector3 AttackLocation,
        Health.AttackTypes AttackType,
        bool FromBehind = false)
    {
        base.OnHitEarly(Attacker, AttackLocation, AttackType, FromBehind);
        if (Attacker == gameObject)
            return;
        if (PlayerController.CanParryAttacks && canBeParried && !FromBehind && AttackType == Health.AttackTypes.Melee)
        {
            health.WasJustParried = true;
            SimpleSpineFlash.FlashWhite(false);
            SeperateObject = true;
            UsePathing = true;
            health.invincible = false;
            StopAllCoroutines();
            DisableForces = false;
        }
    }

    public override void OnDie(
        GameObject Attacker,
        Vector3 AttackLocation,
        Health Victim,
        Health.AttackTypes AttackType,
        Health.AttackFlags AttackFlags)
    {
        var soundPath = DeathVO;

        if (!string.IsNullOrEmpty(soundPath))
            AudioManager.Instance.PlayOneShot(soundPath, transform.position);

        CleanupEventInstances();
        base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    }

    public virtual void CleanupEventInstances()
    {
        //cleanup audio if you have, other events, etc
    }

    public override void OnHit(
        GameObject Attacker,
        Vector3 AttackLocation,
        Health.AttackTypes AttackType,
        bool FromBehind)
    {
        base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);

        if (health.HasShield || health.WasJustParried) return;

        if (damageColliderEvents != null) damageColliderEvents.SetActive(false);

        if (!string.IsNullOrEmpty(onHitSoundPath)) AudioManager.Instance.PlayOneShot(onHitSoundPath, transform.position);

        var soundPath = GetHitVO;
        if (!string.IsNullOrEmpty(soundPath))
            AudioManager.Instance.PlayOneShot(soundPath, transform.position);

        Spine.AnimationState.SetAnimation(1, "hurt-eyes", false); //TODO: be variable

        if (MyState != State.Attacking)
        {
            SimpleSpineFlash.FlashWhite(false);
            SeperateObject = true;
            UsePathing = true;
            health.invincible = false;
            StopAllCoroutines();
            DisableForces = false;

            if (AttackLocation.x > (double)transform.position.x && state.CURRENT_STATE != StateMachine.State.HitRight)
                state.CURRENT_STATE = StateMachine.State.HitRight;

            if (AttackLocation.x < (double)transform.position.x && state.CURRENT_STATE != StateMachine.State.HitLeft)
                state.CURRENT_STATE = StateMachine.State.HitLeft;

            if (AttackType != Health.AttackTypes.Heavy && (!(AttackType == Health.AttackTypes.Projectile & FromBehind) || health.HasShield))
                StartCoroutine(HurtRoutine());
        }
        if (AttackType == Health.AttackTypes.Projectile && !health.HasShield)
        {
            state.facingAngle = state.LookAngle = Utils.GetAngle(transform.position, Attacker.transform.position);
            Spine.skeleton.ScaleX = state.LookAngle <= 90.0 || state.LookAngle >= 270.0 ? -1f : 1f;
        }
        if (AttackType != Health.AttackTypes.NoKnockBack)
            StartCoroutine(ApplyForceRoutine(Attacker));

        SimpleSpineFlash.FlashFillRed();
    }

    public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
    {

        DisableForces = true;
        Force = (transform.position - Attacker.transform.position).normalized * 500f;
        rb.AddForce((Vector2)(Force * KnockbackModifier));

        var time = 0.0f;

        while ((time += Time.deltaTime * Spine.timeScale) < 0.5)
            yield return null;

        DisableForces = false;
    }

    public virtual IEnumerator HurtRoutine()
    {
        var time = 0.0f;

        while ((time += Time.deltaTime * Spine.timeScale) < 0.3)
            yield return null;

        StartCoroutine(WaitForTarget());
    }

    public virtual IEnumerator WaitForTarget()
    {
        while (Spine == null)
            yield return new WaitForEndOfFrame(); ;

        Spine.Initialize(false);

        while (!GameManager.RoomActive)
            yield return null;

        yield return new WaitForEndOfFrame();

        while (TargetObject == null)
        {
            var closestTarget = GetClosestTarget(health.team == Health.Team.PlayerTeam);
            if (closestTarget)
            {
                TargetObject = closestTarget.gameObject;
                requireLineOfSite = false;
                VisionRange = int.MaxValue;
            }

            RepathTimer -= Time.deltaTime * Spine.timeScale;
            if (RepathTimer <= 0.0)
            {
                if (state.CURRENT_STATE == StateMachine.State.Moving)
                {
                    if (Spine.AnimationName != "run") //TODO: become variable
                        Spine.AnimationState.SetAnimation(0, "run", true); //TODO: become variable
                }

                else if (Spine.AnimationName != "idle") //TODO: become variable
                    Spine.AnimationState.SetAnimation(0, "idle", true); //TODO: become variable

                if (!FollowPlayer)
                    TargetPosition = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 2f;

                FindPath(TargetPosition);
                state.LookAngle = Utils.GetAngle(transform.position, TargetPosition);
                Spine.skeleton.ScaleX = state.LookAngle <= 90.0 || state.LookAngle >= 270.0 ? -1f : 1f;
            }

            yield return null;
        }

        var InRange = false;
        while (!InRange)
        {
            if (TargetObject == null)
            {
                StartCoroutine(WaitForTarget());
                yield break;
            }

            var a = Vector3.Distance(TargetObject.transform.position, transform.position);
            if (a <= VisionRange)
            {
                if (!requireLineOfSite || CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, VisionRange)))
                    InRange = true;
                else
                    LookAtTarget();
            }

            yield return null;
        }

        StartCoroutine(ChasePlayer());
    }

    public virtual void LookAtTarget()
    {
        if (GetClosestTarget() == null)
            return;

        var angle = Utils.GetAngle(transform.position, GetClosestTarget().transform.position);

        state.facingAngle = angle;
        state.LookAngle = angle;

        if (!(Spine.AnimationName != "jeer")) //TODO: become variable
            return;

        Spine.randomOffset = true;
        Spine.AnimationState.SetAnimation(0, "jeer", true); //TODO: become variable
    }

    public virtual IEnumerator ChasePlayer()
    {
        MyState = State.WaitAndTaunt;
        state.CURRENT_STATE = StateMachine.State.Idle;
        AttackDelay = UnityEngine.Random.Range(AttackDelayRandomRange.x, AttackDelayRandomRange.y);

        if (health.HasShield)
            AttackDelay = 2.5f;

        MaxAttackDelay = UnityEngine.Random.Range(MaxAttackDelayRandomRange.x, MaxAttackDelayRandomRange.y);

        var Loop = true;
        while (Loop)
        {
            if (TargetObject == null)
            {
                StartCoroutine(WaitForTarget());
                break;
            }

            if (damageColliderEvents != null)
                damageColliderEvents.SetActive(false);

            TeleportDelay -= Time.deltaTime * Spine.timeScale;
            AttackDelay -= Time.deltaTime * Spine.timeScale;
            MaxAttackDelay -= Time.deltaTime * Spine.timeScale;

            if (MyState == State.WaitAndTaunt)
            {
                if (Spine.AnimationName != "roll-stop" && state.CURRENT_STATE == StateMachine.State.Moving) //TODO: become variable
                {

                    if (Spine.AnimationName != "run") //TODO: become variable
                        Spine.AnimationState.SetAnimation(0, "run", true); //TODO: become variable
                }

                else if (Spine.AnimationName != "cheer1") //TODO: become variable
                    Spine.AnimationState.SetAnimation(0, "cheer1", true); //TODO: become variable

                if (TargetObject == PlayerFarming.Instance.gameObject && health.IsCharmed && GetClosestTarget() != null)
                    TargetObject = GetClosestTarget().gameObject;

                state.LookAngle = Utils.GetAngle(transform.position, TargetObject.transform.position);
                Spine.skeleton.ScaleX = state.LookAngle <= 90.0 || state.LookAngle >= 270.0 ? -1f : 1f;

                if (state.CURRENT_STATE == StateMachine.State.Idle)
                {

                    if ((RepathTimer -= Time.deltaTime * Spine.timeScale) < 0.0)
                    {
                        if (CustomAttackLogic())
                            break;

                        if (MaxAttackDelay < 0.0 || Vector3.Distance(transform.position, TargetObject.transform.position) < AttackWithinRange)
                        {
                            AttackWithinRange = float.MaxValue;
                            if ((bool)TargetObject)
                            {
                                if (ChargeAndAttack && (MaxAttackDelay < 0.0 || AttackDelay < 0.0))
                                {
                                    health.invincible = false;
                                    StopAllCoroutines();
                                    DisableForces = false;
                                    StartCoroutine(FightPlayer());
                                }

                                else if (health.HasShield)
                                {
                                    Angle = (float)((Utils.GetAngle(TargetObject.transform.position, transform.position) + UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                                    TargetPosition = TargetObject.transform.position + new Vector3(MaintainTargetDistance * Mathf.Cos(Angle), MaintainTargetDistance * Mathf.Sin(Angle));
                                    FindPath(TargetPosition);
                                }
                            }
                        }

                        else if ((bool)TargetObject && Vector3.Distance(transform.position, TargetObject.transform.position) > MoveCloserDistance + (health.HasShield ? 0.0 : 1.0))
                        {
                            Angle = (float)((Utils.GetAngle(TargetObject.transform.position, transform.position) + UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                            TargetPosition = TargetObject.transform.position + new Vector3(MaintainTargetDistance * Mathf.Cos(Angle), MaintainTargetDistance * Mathf.Sin(Angle));
                            FindPath(TargetPosition);
                        }
                    }
                }
                else if ((RepathTimer += Time.deltaTime * Spine.timeScale) > 2.0)
                {
                    RepathTimer = 0.0f;
                    state.CURRENT_STATE = StateMachine.State.Idle;
                }

            }

            Seperate(0.5f);
            yield return null;
        }
    }

    public override void BeAlarmed(GameObject TargetObject)
    {
        base.BeAlarmed(TargetObject);

        if (!string.IsNullOrEmpty(DrawbackSwordSFX))
            AudioManager.Instance.PlayOneShot(DrawbackSwordSFX, transform.position);

        this.TargetObject = TargetObject;

        var a = Vector3.Distance(TargetObject.transform.position, transform.position);
        if ((double)a > VisionRange)
            return;

        if (!requireLineOfSite || CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, VisionRange)))
            StartCoroutine(WaitForTarget());
        else
            LookAtTarget();
    }

    public virtual bool CustomAttackLogic() => false;

    public virtual void FindPath(Vector3 PointToCheck)
    {
        RepathTimer = 0.2f;
        var raycastHit2D = Physics2D.CircleCast((Vector2)transform.position, 0.2f, (Vector2)Vector3.Normalize(PointToCheck - transform.position), NewPositionDistance, (int)layerToCheck);
        if (raycastHit2D.collider != null)
        {
            if ((double)Vector3.Distance(transform.position, (Vector3)raycastHit2D.centroid) > 1.0)
            {
                if (ShowDebug)
                {
                    Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(transform.position - PointToCheck) * CircleCastOffset);
                    PointsLink.Add(new Vector3(transform.position.x, transform.position.y));
                }
                TargetPosition = (Vector3)raycastHit2D.centroid + Vector3.Normalize(transform.position - PointToCheck) * CircleCastOffset;
                givePath(TargetPosition);
            }
            else
            {
                if (TeleportDelay >= 0.0)
                    return;
                Teleport();
            }
        }
        else if (FollowPlayer && PlayerFarming.Instance != null)
        {
            if (Vector3.Distance(transform.position, PlayerFarming.Instance.transform.position) <= 1.5)
                return;

            TargetPosition = PlayerFarming.Instance.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle;
            givePath(TargetPosition);
        }
        else
        {
            TargetPosition = PointToCheck;
            givePath(PointToCheck);
        }
    }

    public virtual void Teleport()
    {
        if (MyState != State.WaitAndTaunt || health.HP <= 0.0)
            return;

        var num1 = 100f;
        float num2;

        if ((num2 = num1 - 1f) <= 0.0 || TargetObject == null)
            return;

        var f = (float)((Utils.GetAngle(transform.position, TargetObject.transform.position) + UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
        var distance = 4.5f;
        var radius = 0.2f;

        var Position = TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        var raycastHit2D = Physics2D.CircleCast((Vector2)transform.position, radius, (Vector2)Vector3.Normalize(Position - transform.position), distance, (int)layerToCheck);

        if (raycastHit2D.collider != null)
        {
            if (ShowDebug)
            {
                Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(transform.position - Position) * CircleCastOffset);
                PointsLink.Add(new Vector3(transform.position.x, transform.position.y));
            }
            StartCoroutine(TeleportRoutine((Vector3)raycastHit2D.centroid));
        }
        else
        {
            if (ShowDebug)
            {
                EndPoints.Add(new Vector3(Position.x, Position.y));
                EndPointsLink.Add(new Vector3(transform.position.x, transform.position.y));
            }
            StartCoroutine(TeleportRoutine(Position));
        }
    }

    public virtual IEnumerator TeleportRoutine(Vector3 Position)
    {
        ClearPaths();

        state.CURRENT_STATE = StateMachine.State.Moving;
        UsePathing = false;
        health.invincible = true;
        SeperateObject = false;
        MyState = State.Teleporting;

        ClearPaths();

        var position = transform.position;
        var Progress = 0.0f;

        Spine.AnimationState.SetAnimation(0, "roll", true); //TODO: become variable
        state.facingAngle = state.LookAngle = Utils.GetAngle(transform.position, Position);
        Spine.skeleton.ScaleX = state.LookAngle <= 90.0 || state.LookAngle >= 270.0 ? -1f : 1f;

        var b = Position;
        var Duration = Vector3.Distance(position, b) / 10f;

        AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/swordsman_mutated/mv_roll", gameObject); //TODO: become variable

        while ((Progress += Time.deltaTime * Spine.timeScale) < Duration)
        {
            speed = 10f * Time.deltaTime * Spine.timeScale;
            yield return null;
        }

        state.CURRENT_STATE = StateMachine.State.Idle;
        Spine.AnimationState.SetAnimation(0, "roll-stop", false); //TODO: become variable
        var time = 0.0f;

        while ((time += Time.deltaTime * Spine.timeScale) < 0.3)
            yield return null;
        UsePathing = true;
        RepathTimer = 0.5f;
        TeleportDelay = TeleportDelayTarget;
        SeperateObject = true;
        health.invincible = false;
        MyState = State.WaitAndTaunt;
    }

    public virtual void GraveSpawn(bool longAnim = false)
    {
        StopAllCoroutines();
        StartCoroutine(GraveSpawnRoutine(longAnim));
    }

    public virtual IEnumerator GraveSpawnRoutine(bool longAnim = false)
    {
        health.invincible = true;
        state.CURRENT_STATE = StateMachine.State.CustomAnimation;

        yield return new WaitForEndOfFrame();

        Spine.AnimationState.SetAnimation(0, longAnim ? "grave-spawn-long" : "grave-spawn", false); //TODO: become variable
        Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f); //TODO: become variable

        yield return new WaitForSeconds(1.5f);

        health.invincible = false;
        state.CURRENT_STATE = StateMachine.State.Idle;

        StartCoroutine(WaitForTarget());
    }

    public virtual void OnDrawGizmos()
    {
        Utils.DrawCircleXY(transform.position, VisionRange, Color.yellow);

        var index1 = -1;

        while (++index1 < Points.Count)
        {
            Utils.DrawCircleXY(PointsLink[index1], 0.5f, Color.blue);
            Utils.DrawCircleXY(Points[index1], CircleCastRadius, Color.blue);
            Utils.DrawLine(Points[index1], PointsLink[index1], Color.blue);
        }
        var index2 = -1;

        while (++index2 < EndPoints.Count)
        {
            Utils.DrawCircleXY(EndPointsLink[index2], 0.5f, Color.red);
            Utils.DrawCircleXY(EndPoints[index2], CircleCastRadius, Color.red);
            Utils.DrawLine(EndPointsLink[index2], EndPoints[index2], Color.red);
        }
    }

    public virtual IEnumerator FightPlayer(float AttackDistance = 1.5f)
    {
        MyState = State.Attacking;
        UsePathing = true;

        givePath(TargetObject.transform.position);
        Spine.AnimationState.SetAnimation(0, "run-charge", true); //TODO: become variable

        if (!string.IsNullOrEmpty(DrawbackSwordSFX))
            AudioManager.Instance.PlayOneShot(DrawbackSwordSFX, gameObject);

        var distanceResetTimer = 0.0f;
        var previousPosition = (Vector2)transform.position;
        RepathTimer = 0.0f;
        var NumAttacks = DoubleAttack ? 2 : 1;
        var AttackCount = 1;
        var MaxAttackSpeed = 15f;
        var AttackSpeed = MaxAttackSpeed;
        var Loop = true;
        var SignPostDelay = 0.5f;

        while (Loop)
        {
            if (Spine == null || Spine.AnimationState == null || Spine.Skeleton == null)
            {
                yield return null;
            }
            else
            {
                Seperate(0.5f);
                switch (state.CURRENT_STATE)
                {
                    case StateMachine.State.Idle:
                        TargetObject = null;
                        StartCoroutine(WaitForTarget());
                        yield break;
                    case StateMachine.State.Moving:
                        if (TargetObject != null)
                        {
                            state.LookAngle = Utils.GetAngle(transform.position, TargetObject.transform.position);
                            Spine.skeleton.ScaleX = state.LookAngle <= 90.0 || state.LookAngle >= 270.0 ? -1f : 1f;
                            state.LookAngle = state.facingAngle = Utils.GetAngle(transform.position, TargetObject.transform.position);
                        }
                        if (TargetObject != null && Vector2.Distance((Vector2)transform.position, (Vector2)TargetObject.transform.position) < AttackDistance)
                        {
                            state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                            Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-charge2" : "grunt-attack-charge", false); //TODO: become variable
                        }
                        else
                        {
                            if ((RepathTimer += Time.deltaTime * Spine.timeScale) > 0.2 && TargetObject != null)
                            {
                                RepathTimer = 0.0f;
                                givePath(TargetObject.transform.position);
                            }

                            if (FollowPlayer && TargetObject != null)
                            {
                                var magnitude = ((Vector2)transform.position - previousPosition).magnitude;
                                previousPosition = (Vector2)transform.position;

                                if (magnitude <= 0.14)
                                {
                                    distanceResetTimer += Time.deltaTime * Spine.timeScale;
                                    if ((double)distanceResetTimer > 6.5)
                                        transform.position = PlayerFarming.Instance.transform.position;
                                }
                                else
                                    distanceResetTimer = 0.0f;
                            }
                            if (damageColliderEvents != null)
                            {
                                if (state.Timer < 0.2 && !health.WasJustParried)
                                    damageColliderEvents.SetActive(true);
                                else
                                    damageColliderEvents.SetActive(false);
                            }
                        }
                        if (damageColliderEvents != null)
                        {
                            damageColliderEvents.SetActive(false);
                            break;
                        }
                        break;
                    case StateMachine.State.SignPostAttack:
                        if (TargetObject == null)
                        {
                            Spine.AnimationState.SetAnimation(0, "idle", true); //TODO: become variable
                            SimpleSpineFlash.FlashWhite(false);
                            state.CURRENT_STATE = StateMachine.State.Idle;
                            break;
                        }

                        if (damageColliderEvents != null)
                            damageColliderEvents.SetActive(false);

                        SimpleSpineFlash.FlashWhite(state.Timer / SignPostDelay);
                        state.Timer += Time.deltaTime * Spine.timeScale;

                        if (state.Timer >= (double)SignPostDelay - signPostParryWindow)
                            canBeParried = true;

                        if (state.Timer >= (double)SignPostDelay)
                        {
                            SimpleSpineFlash.FlashWhite(false);
                            CameraManager.shakeCamera(0.4f, state.LookAngle);
                            state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;

                            speed = AttackSpeed * 0.0166666675f;
                            Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-impact2" : "grunt-attack-impact", false); //TODO: become variable

                            canBeParried = true;

                            StartCoroutine(EnableDamageCollider(0.0f));

                            if (!string.IsNullOrEmpty(attackSoundPath))
                                AudioManager.Instance.PlayOneShot(attackSoundPath, transform.position);

                            if (!string.IsNullOrEmpty(AttackVO))
                            {
                                AudioManager.Instance.PlayOneShot(AttackVO, transform.position);
                                break;
                            }

                            break;
                        }
                        break;
                    case StateMachine.State.RecoverFromAttack:
                        if (AttackSpeed > 0.0)
                            AttackSpeed -= 1f * GameManager.DeltaTime * Spine.timeScale;

                        speed = AttackSpeed * Time.deltaTime * Spine.timeScale;
                        SimpleSpineFlash.FlashWhite(false);
                        canBeParried = state.Timer <= attackParryWindow;

                        if ((state.Timer += Time.deltaTime * Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
                        {
                            if (++AttackCount <= NumAttacks)
                            {
                                AttackSpeed = MaxAttackSpeed + (3 - NumAttacks) * 2;
                                state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                                Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false); //TODO: become variable
                                SignPostDelay = 0.3f;
                                break;
                            }
                            Loop = false;
                            SimpleSpineFlash.FlashWhite(false);
                            break;
                        }
                        break;
                }
                yield return null;
            }
        }

        TargetObject = null;
        StartCoroutine(WaitForTarget());
    }

    public virtual IEnumerator PlaceIE()
    {
        ClearPaths();

        var offset = (Vector3)UnityEngine.Random.insideUnitCircle;
        while (PlayerFarming.Instance.GoToAndStopping)
        {
            state.CURRENT_STATE = StateMachine.State.Moving;
            var vector3 = (PlayerFarming.Instance.transform.position + offset) with
            {
                z = 0.0f
            };
            transform.position = vector3;
            yield return null;
        }

        state.CURRENT_STATE = StateMachine.State.Idle;
        Spine.AnimationState.SetAnimation(0, "idle-enemy", true); //TODO: become variable
    }

    public virtual void OnDamageTriggerEnter(Collider2D collider)
    {
        EnemyHealth = collider.GetComponent<Health>();
        if (!(EnemyHealth != null))
            return;
        if (EnemyHealth.team != health.team)
        {
            EnemyHealth.DealDamage(Damage, gameObject, Vector3.Lerp(transform.position, EnemyHealth.transform.position, 0.7f));
        }
        else
        {
            if (health.team != Health.Team.PlayerTeam || health.isPlayerAlly || EnemyHealth.isPlayer)
                return;
            EnemyHealth.DealDamage(Damage, gameObject, Vector3.Lerp(transform.position, EnemyHealth.transform.position, 0.7f));
        }
    }

    public virtual IEnumerator EnableDamageCollider(float initialDelay, float duration = 0.2f, System.Action callback = null)
    {
        if (damageColliderEvents)
        {
            var time = 0.0f;
            while ((double)(time += Time.deltaTime * Spine.timeScale) < (double)initialDelay)
                yield return null;

            damageColliderEvents.SetActive(true);

            time = 0.0f;
            while ((double)(time += Time.deltaTime * Spine.timeScale) < (double)duration)
                yield return null;

            damageColliderEvents.SetActive(false);
        }

        callback?.Invoke();
    }

    public virtual void BiomeGenerator_OnBiomeChangeRoom()
    {
        if (!(PlayerFarming.Instance != null) || !FollowPlayer)
            return;

        ClearPaths();
        state.CURRENT_STATE = StateMachine.State.Idle;
        Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
    }

    public virtual void ResetResilience()
    {
        LogInfo("Resetting resilience for " + gameObject.name);
    }

    public virtual void StopResilience()
    {
        LogInfo("Stopping resilience for " + gameObject.name);
    }

    public enum State
    {
        WaitAndTaunt,
        Teleporting,
        Attacking,
    }
}
