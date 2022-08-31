export const SITE = {
    title: "COLT_API's Documentation",
    description: "Cult of the Lamb API's Documentation"
};

export const OPEN_GRAPH = {
    image: {
        src: "https://github.com/withastro/astro/blob/main/assets/social/banner-minimal.png?raw=true",
        alt:
            "astro logo on a starry expanse of space," +
            " with a purple saturn-like planet floating in the right foreground"
    },
    twitter: "hayper1919"
};

// Uncomment this to add an "Edit this page" button to every page of documentation.
export const GITHUB_EDIT_URL = `https://github.com/xhayper/COTL_API/tree/gh-pages/`;

// Uncomment this to add an "Join our Community" button to every page of documentation.
export const COMMUNITY_INVITE_URL = `https://discord.gg/jZ2DytX3TX`;

// Uncomment this to enable site search.
// See "Algolia" section of the README for more information.
// export const ALGOLIA = {
//   indexName: 'XXXXXXXXXX',
//   appId: 'XXXXXXXXXX',
//   apiKey: 'XXXXXXXXXX',
// }

export const SIDEBAR = [
    { text: "Introduction", header: true },
    { text: "Main Page", link: "COTL_API/introduction" },
    { text: "Getting Started", link: "COTL_API/getting-started" },

    { text: "Features", header: true },
    { text: "Items", link: "COTL_API/items" },
    { text: "Follower Commands", link: "COTL_API/follower-commands" },
    { text: "Save File Data", link: "COTL_API/save-data" }
];
