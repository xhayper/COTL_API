import { options, Fragment, Component } from "preact";
import { version } from "preact/compat";

export function initDevTools() {
    if (typeof window === "undefined") return;
    if (!("__PREACT_DEVTOOLS__" in window)) return;

    (window as any).__PREACT_DEVTOOLS__.attachPreact(version, options, {
        Fragment,
        Component
    });
}
