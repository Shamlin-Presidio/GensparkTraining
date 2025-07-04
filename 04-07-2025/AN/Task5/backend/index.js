const express = require('express');
const mongoose = require('mongoose');
const app = express();
const PORT = 3000;

mongoose.connect(process.env.MONGO_URL, {
  useNewUrlParser: true,
  useUnifiedTopology: true,
})
.then(() => console.log('Connected to MongoDB'))
.catch(err => console.error('Mongo error:', err));

app.get('/', (req, res) => {
  res.send('Hello from Node.js API!');
});

app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
