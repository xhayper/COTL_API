import { defineConfig } from "astro/config";
import remarkGithub from "remark-github";
import preact from "@astrojs/preact";
import react from "@astrojs/react";

// https://astro.build/config
export default defineConfig({
  integrations: [
    // Enable Preact to support Preact JSX components.
    preact(),
    // Enable React for the Algolia search component.
    react(),
  ],
  markdown: {
    extendDefaultPlugins: true,
    remarkPlugins: [
      remarkGithub({
        repository: "https://github.com/xhayper/COTL_API.git",
      }),
    ],
  },
  site: "https://xhayper.github.io",
  base: "/COTL_API",
});
