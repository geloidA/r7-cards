/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,html,cshtml}"],
  theme: {
    extend: {
      colors: {
        'inverse': 'var(--neutral-fill-inverse-rest)',
        'accent': 'var(--accent-fill-rest)',
        'neutral-hover': 'var(--neutral-fill-hover)'
      }
    },
  },
  plugins: [],
}

