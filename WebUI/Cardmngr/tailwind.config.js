/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,html,cshtml}"],
  theme: {
    extend: {
      width: {
        'details-popup': '348px',
      },
      colors: {
        'inverse': 'var(--neutral-fill-inverse-rest)',
        'accent': 'var(--accent-fill-rest)',
        'neutral-hover': 'var(--neutral-fill-hover)',
        'success': 'var(--success)',
        'error': 'var(--error)',
        'warning': 'var(--warning)',
        'disabled': 'var(--neutral-stroke-rest)',
        'info': 'var(--info)',
        'forground-on-accent': 'var(--foreground-on-accent-rest)',
        'layer-1': 'var(--neutral-layer-1)',
        'layer-2': 'var(--neutral-layer-2)',
        'layer-3': 'var(--neutral-layer-3)',
        'layer-4': 'var(--neutral-layer-4)'
      }
    },
  },
  plugins: [],
}

