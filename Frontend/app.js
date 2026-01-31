const baseUrlInput = document.getElementById('base-url');
const apiUrlSpan = document.getElementById('api-url');
const saveBaseBtn = document.getElementById('save-base');
const registerForm = document.getElementById('register-form');
const loginForm = document.getElementById('login-form');
const registerStatus = document.getElementById('register-status');
const loginStatus = document.getElementById('login-status');
const sessionInfo = document.getElementById('session-info');
const clearSessionBtn = document.getElementById('clear-session');

const storageKey = 'ebutiken_base_url';
const tokenKey = 'ebutiken_token';

function setApiUrl(url) {
  localStorage.setItem(storageKey, url);
  apiUrlSpan.textContent = url;
}

function getApiUrl() {
  return localStorage.getItem(storageKey) || baseUrlInput.value.trim();
}

function setToken(token) {
  localStorage.setItem(tokenKey, token);
  sessionInfo.textContent = `Logged in. Token: ${token}`;
}

function clearToken() {
  localStorage.removeItem(tokenKey);
  sessionInfo.textContent = 'Not logged in.';
}

function showStatus(el, msg, isError = false) {
  el.textContent = msg;
  el.classList.toggle('error', isError);
  el.classList.toggle('success', !isError);
}

async function postJson(path, body) {
  const res = await fetch(`${getApiUrl()}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  });
  const text = await res.text();
  let data;
  try { data = text ? JSON.parse(text) : null; } catch { data = text; }
  return { ok: res.ok, status: res.status, data };
}

// Init
(function init() {
  const saved = localStorage.getItem(storageKey);
  if (saved) {
    baseUrlInput.value = saved;
  }
  setApiUrl(baseUrlInput.value.trim());
  const savedToken = localStorage.getItem(tokenKey);
  if (savedToken) setToken(savedToken);
})();

saveBaseBtn.addEventListener('click', () => {
  setApiUrl(baseUrlInput.value.trim());
});

registerForm.addEventListener('submit', async (e) => {
  e.preventDefault();
  showStatus(registerStatus, 'Registering...');
  const form = new FormData(registerForm);
  const payload = {
    name: form.get('name'),
    email: form.get('email'),
    password: form.get('password')
  };
  const { ok, status, data } = await postJson('/register', payload);
  if (ok) {
    showStatus(registerStatus, 'Registered!', false);
  } else {
    const message = data?.message || JSON.stringify(data) || status;
    showStatus(registerStatus, `Failed: ${message}`, true);
  }
});

loginForm.addEventListener('submit', async (e) => {
  e.preventDefault();
  showStatus(loginStatus, 'Logging in...');
  const form = new FormData(loginForm);
  const payload = {
    email: form.get('email'),
    password: form.get('password')
  };
  const { ok, status, data } = await postJson('/login', payload);
  if (ok) {
    const token = data?.token || `user-${payload.email}`;
    setToken(token);
    showStatus(loginStatus, 'Logged in!', false);
  } else {
    const message = data?.message || JSON.stringify(data) || status;
    showStatus(loginStatus, `Failed: ${message}`, true);
  }
});

clearSessionBtn.addEventListener('click', () => {
  clearToken();
  showStatus(loginStatus, '');
});
