export const SITE = {
    title: "COTL_API's Documentation",
    description: "Cult of the Lamb API's Documentation"
};

export const SEO = {
    opengraph: {
        image: {
            src: "https://cotl-api.vercel.app/banner.png",
            alt: "Cult of the Lamb (English/Chinese/Korean/Japanese Ver.)",
            width: 3840,
            height: 2160,
            mimeType: "image/png"
        }
    },
    twitter: {
        card: "summary_large_image",
        site: "@hayper1919",
        creator: "@hayper1919"
    }
};

// Uncomment this to add an "Edit this page" button to every page of documentation.
export const GITHUB_EDIT_URL = `https://github.com/xhayper/COTL_API/tree/docs/`;

// Uncomment this to add an "Join our Community" button to every page of documentation.
export const COMMUNITY_INVITE_URL = `https://discord.gg/jZ2DytX3TX`;

// Uncomment this to enable site search.
// See "Algolia" section of the README for more information.
// export const ALGOLIA = {
//   indexName: 'XXXXXXXXXX',
//   appId: 'XXXXXXXXXX',
//   apiKey: 'XXXXXXXXXX',
// }

export const SIDEBAR: { text: string; header?: boolean; link?: string }[] = [
    { text: "Introduction", header: true },
    { text: "Main Page", link: "" },
    { text: "Getting Started", link: "getting-started" },

    { text: "Features", header: true },
    { text: "Items", link: "items" },
    { text: "Follower Commands", link: "follower-commands" },
    { text: "Objectives", link: "objectives" },
    { text: "Save File Data", link: "save-data" }
];
