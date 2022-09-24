import { defineConfig } from "astro/config";
import sitemap from "@astrojs/sitemap";
import preact from "@astrojs/preact";
import react from "@astrojs/react";
import partytown from "@astrojs/partytown";

// https://astro.build/config
export default defineConfig({
  integrations: [preact(), react(), sitemap(), partytown({
    config: {
      debug: true
    }
  })],
  vite: {
    build: {
      sourcemap: true
    }
  },
  site: "https://xhayper.github.io",
  base: "/COTL_API"
});