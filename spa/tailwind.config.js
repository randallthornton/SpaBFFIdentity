/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      animation: {
        "fly-in-bottom": "fly-in-bottom 400ms cubic-bezier(0.05, .7, .1, 1)",
        "will-change": "transform",
      },
      keyframes: {
        "fly-in-bottom": {
          "0%": {
            opacity: "0",
            transform: "translateY(100%)",
            "-webkit-transform": "translateY(100%)",
            "-moz-transform": "translateY(100%)",
            "-o-transform": "translateY(100%)",
          },
          "100%": {
            opacity: "1",
            transform: "translateY(0)",
            "-webkit-transform": "translateY(0)",
            "-moz-transform": "translateY(0)",
            "-o-transform": "translateY(0)",
          },
        },
      },
    },
  },
  plugins: [],
};
