import starlightPlugin from "@astrojs/starlight-tailwind";
import colors from "tailwindcss/colors";

/** @type {import('tailwindcss').Config} */
export default {
  content: ["./src/**/*.{astro,html,js,jsx,md,mdx,svelte,ts,tsx,vue}"],
  theme: {
    extend: {
      colors: {
        accent: colors.indigo,
        gray: colors.zinc
      },
      fontFamily: {
        sans: ['"Inter Variable", "Inter", sans-serif'],
        mono: ['"JetBrains Mono Variable", "JetBrains Mono", monospace']
      }
    }
  },
  plugins: [starlightPlugin()]
};
