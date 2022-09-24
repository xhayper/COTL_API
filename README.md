# Astro Starter Kit: Docs Site

```bash
yarn init astro -- --template docs
```

[![Open in StackBlitz](https://developer.stackblitz.com/img/open_in_stackblitz.svg)](https://stackblitz.com/github/withastro/astro/tree/latest/examples/docs)

![docs](https://user-images.githubusercontent.com/4677417/186189283-0831b9ab-d6b9-485d-8955-3057e532ab31.png)

## Features

-   ✅ **Full Markdown support**
-   ✅ **Responsive mobile-friendly design**
-   ✅ **Sidebar navigation**
-   ✅ **Search (powered by Algolia)**
-   ✅ **Automatic table of contents**
-   ✅ **Automatic list of contributors**
-   ✅ (and, best of all) **dark mode**

## Commands Cheatsheet

All commands are run from the root of the project, from a terminal:

| Command                 | Action                                           |
| :---------------------- | :----------------------------------------------- |
| `yarn install`          | Installs dependencies                            |
| `yarn run dev`          | Starts local dev server at `localhost:3000`      |
| `yarn run build`        | Build your production site to `./dist/`          |
| `yarn run preview`      | Preview your build locally, before deploying     |
| `yarn run astro ...`    | Run CLI commands like `astro add`, `astro check` |
| `yarn run astro --help` | Get help using the Astro CLI                     |

To deploy your site to production, check out our [Deploy an Astro Website](https://docs.astro.build/guides/deploy) guide.

## New to Astro?

Welcome! Check out [our documentation](https://docs.astro.build) or jump into our [Discord server](https://astro.build/chat).

## Customize This Theme

### Site metadata

`src/config.ts` contains several data objects that describe metadata about your site like title, description, and Open Graph details. You can customize these to match your project.

### CSS styling

The theme's look and feel is controlled by a few key variables that you can customize yourself. You'll find them in the `public/theme.scss` CSS file.

If you've never worked with CSS variables before, give [MDN's guide on CSS variables](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties) a quick read.

This theme uses a "cool blue" accent color by default. To customize this for your project, change the `--theme-accent` variable to whatever color you'd like:

```diff
/* public/theme.scss */
:root {
  color-scheme: light;
-  --theme-accent: hsla(var(--color-blue), 1);
+  --theme-accent: hsla(var(--color-red), 1);   /* or: hsla(#FF0000, 1); */
```

## Page metadata

Astro uses frontmatter in Markdown pages to choose layouts and pass properties to those layouts. If you are using the default layout, you can customize the page in many different ways to optimize SEO and other things. For example, you can use the `title` and `description` properties to set the document title, meta title, meta description, and Open Graph description.

```markdown
---
title: Example title
description: Really cool docs example that uses Astro
layout: ../layouts/MainLayout.astro
---

# Page content...
```

For more SEO related properties, look at `src/components/HeadSEO.astro`

### Sidebar navigation

The sidebar navigation is controlled by the `SIDEBAR` variable in your `src/config.ts` file. You can customize the sidebar by modifying this object. A default, starter navigation has already been created for you.

```ts
export const SIDEBAR = [
    { text: "Section Header", header: true },
    { text: "Introduction", link: "COTL_API/introduction" },
    { text: "Page 2", link: "COTL_API/page-2" },
    { text: "Page 3", link: "COTL_API/page-3" },

    { text: "Another Section", header: true },
    { text: "Page 4", link: "COTL_API/page-4" }
];
```

### Search (Powered by Algolia)

[Algolia](https://www.algolia.com/) offers a free service to qualified open source projects called [DocSearch](https://docsearch.algolia.com/). If you are accepted to the DocSearch program, provide your API Key & index name in `src/config.ts` and a search box will automatically appear in your site header.

Note that Aglolia and Astro are not affiliated. We have no say over acceptance to the DocSearch program.

If you'd prefer to remove Algolia's search and replace it with your own, check out the `src/components/Header.astro` component to see where the component is added.
