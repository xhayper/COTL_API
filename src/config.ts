export const SITE = {
    title: "Cult of the Lamb API Documentation",
    description: "Documentation for Cult of the Lamb API",
    defaultLanguage: "en-US"
};

export const OPEN_GRAPH = {
    image: {
        src: "https://cotl-api.vercel.app/banner.png",
        secure_url: "https://cotl-api.vercel.app/banner.png",
        type: "image/png",
        width: 3840,
        height: 2160,
        alt: "Cult of the Lamb (English/Chinese/Korean/Japanese Ver.)"
    }
};

export const TWITTER = {
    card: "summary_large_image",
    site: "@hayper1919",
    creator: "@hayper1919"
};

// This is the type of the frontmatter you put in the docs markdown files.
export type Frontmatter = {
    title: string;
    description: string;
    layout: string;
    image?: { src: string; alt: string };
    dir?: "ltr" | "rtl";
    ogLocale?: string;
    lang?: string;
};

export const GITHUB_EDIT_URL = `https://github.com/xhayper/COTL_API/tree/docs/`;

export const COMMUNITY_INVITE_URL = `https://discord.gg/jZ2DytX3TX`;

export const ALGOLIA = {
    indexName: "XXXXXXXXXX",
    appId: "XXXXXXXXXX",
    apiKey: "XXXXXXXXXX"
};

export type Sidebar = Record<string, { text: string; link: string }[]>;
export const SIDEBAR: Sidebar = {
    Introduction: [
        { text: "Main Page", link: "introduction" },
        { text: "Getting Started", link: "getting-started" }
    ],
    Features: [
        { text: "Follower Commands", link: "follower-commands" },
        { text: "Items", link: "items" },
        { text: "Localization", link: "localization" },
        { text: "Missions", link: "missions" },
        { text: "Objectives", link: "objectives" },
        { text: "Rituals", link: "rituals" },
        { text: "Save Data", link: "save-data" },
        { text: "Settings", link: "settings" },
        { text: "Skins", link: "skins" },
        { text: "Sound", link: "sound" },
        { text: "Structures", link: "structures" },
        { text: "Tarot Cards", link: "tarot-cards" },
        { text: "Tasks", link: "tasks" },
        { text: "UI", link: "ui" }
    ]
};
