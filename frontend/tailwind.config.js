/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#3f51b5',
          light: '#757de8',
          dark: '#002984',
        },
        accent: {
          DEFAULT: '#ff4081',
          light: '#ff79b0',
          dark: '#c60055',
        },
      },
    },
  },
  plugins: [],
}
