import vercel from "@astrojs/vercel/serverless";
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";

const site = "https://cotl-api.vercel.app/";

// https://astro.build/config
export default defineConfig({
  site,
  output: "server",
  adapter: vercel({
    webAnalytics: { enabled: true }
  }),
  integrations: [
    starlight({
      title: "COTL_API",
      logo: {
        src: "./src/assets/logo.svg"
      },
      editLink: {
        baseUrl: "https://github.com/xhayper/COTL_API/edit/main/docs"
      },
      social: {
        github: "https://github.com/xhayper/COTL_API",
        discord: "https://discord.gg/jZ2DytX3TX"
      },
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
      lastUpdated: true,
      credits: true
    })
  ]
});
