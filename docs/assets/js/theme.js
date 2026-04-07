// Theme Toggle Script
document.addEventListener('DOMContentLoaded', () => {
  const theme = localStorage.getItem('theme') || 'light';
  document.documentElement.setAttribute('data-theme', theme);
  updateToggleButtons(theme);
  
  document.querySelectorAll('.theme-toggle button').forEach(btn => {
    btn.addEventListener('click', () => {
      const newTheme = btn.dataset.theme;
      document.documentElement.setAttribute('data-theme', newTheme);
      localStorage.setItem('theme', newTheme);
      updateToggleButtons(newTheme);
    });
  });
  
  function updateToggleButtons(theme) {
    document.querySelectorAll('.theme-toggle button').forEach(btn => {
      btn.classList.toggle('active', btn.dataset.theme === theme);
    });
  }
});
