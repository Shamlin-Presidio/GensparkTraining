import React, { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('');

  useEffect(() => {
    fetch('http://backend:3000/api')
      .then(res => res.json())
      .then(data => setMessage(data.message))
      .catch(err => console.error('Error:', err));
  }, []);

  return (
    <div style={{ textAlign: 'center', marginTop: '100px' }}>
      <h1>Frontend App</h1>
      <h2>Message from Backend:</h2>
      <p>{message}</p>
    </div>
  );
}

export default App;
