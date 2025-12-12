const API_URL = 'http://localhost:5127/api/admin'; 

export const loginAdmin = async (username, password) => {
  const res = await fetch(`${API_URL}/login`, {  
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ username, password })
  });

  if (!res.ok) {
    const error = await res.json();
    throw new Error(error.message || 'Login failed');
  }

  return res.json();
};