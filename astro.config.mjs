import catppuccin from "@catppuccin/starlight";
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";
import vercel from "@astrojs/vercel";

import sitemap from "@astrojs/sitemap";

const site = "https://cotl-api.vercel.app/";

// https://astro.build/config
export default defineConfig({
  site,

  output: "server",

  adapter: vercel({
    webAnalytics: {
      enabled: true
    }
  }),

  integrations: [
    starlight({
      title: "COTL_API",
      logo: {
        src: "./src/assets/logo.svg"
      },
      editLink: {
        baseUrl: "https://github.com/xhayper/COTL_API/tree/docs"
      },
      social: [
        {
          icon: "github",
          label: "GitHub",
          href: "https://github.com/xhayper/COTL_API"
        },
        {
          icon: "discord",
          label: "Discord",
          href: "https://discord.gg/jZ2DytX3TX"
        }
      ],
      head: [
        {
          tag: "meta",
          attrs: {
            property: "og:image",
            content: `${site}img/banner.png`
          }
        },
        {
          tag: "meta",
          attrs: {
            property: "twitter:image",
            content: `${site}img/banner.png`
          }
        }
      ],
      sidebar: [
        {
          label: "Getting Started",
          autogenerate: {
            directory: "getting-started"
          }
        },
        {
          label: "Reference",
          autogenerate: {
            directory: "reference"
          }
        }
      ],
      components: {
        Head: "./src/components/Head.astro"
      },
      lastUpdated: true,
      credits: true,
      plugins: [catppuccin()]
    }),
    sitemap()
  ]
});
