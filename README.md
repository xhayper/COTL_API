# cotldev
Documentation website for the unofficial Cult of the Lamb API. Created using [Docsify](https://docsify.js.org/).

## Contributing

Documentation should be written in markdown and placed in the `docs` folder. The `docs` folder is the root of the website, and the `index.md` file will be the homepage of the website. `_sidebar.md` is the sidebar of the website and contains links to all the other pages. Remember to add links to the sidebar for any new pages you create.

To preview the site locally, it is recommended that you install the `docsify` CLI tool:

```bash
npm i docsify-cli -g
```

Then, from the root of the repository, run:

```bash
docsify serve docs
```

This will start a local server on port 3000. You can then navigate to `http://localhost:3000` to view the site.