/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  darkMode: 'class', // or 'media' if you prefer automatic
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#0F172A',     // slate-900
          light: '#1E293B',
          dark: '#0B1120',
        },
        accent: {
          DEFAULT: '#F59E0B',     // amber-500
          light: '#FBBF24',
          dark: '#B45309',
        },
        background: {
          DEFAULT: '#F8FAFC',     // slate-50
          dark: '#1E293B',
        },
        surface: {
          DEFAULT: '#FFFFFF',
          dark: '#334155',
        },
      },
      fontFamily: {
        sans: ['Inter', 'ui-sans-serif', 'system-ui'],
      },
      spacing: {
        '128': '32rem',
        '144': '36rem',
      },
      boxShadow: {
        soft: '0 4px 12px rgba(0, 0, 0, 0.05)',
        deep: '0 8px 30px rgba(0, 0, 0, 0.12)',
      },
      borderRadius: {
        xl: '1rem',
        '2xl': '1.5rem',
      },
    },
  },
  plugins: [],
}
