import { initDevTools } from "./devtools";
import { options } from "preact";

initDevTools();

/**
 * Display a custom label for a custom hook for the devtools panel
 */
export function addHookName<T>(value: T, name: string): T {
    if (options._addHookName) options._addHookName(name);
    return value;
}
