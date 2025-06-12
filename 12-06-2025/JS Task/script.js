
    const users = [
      { name: "Alice", email: "alice@example.com" },
      { name: "Bob", email: "bob@example.com" },
      { name: "Charlie", email: "charlie@example.com" }
    ];

    // callback
    function fetchUsersCallback(callback) {
      setTimeout(() => {
        callback(users);
      }, 500);
    }

    function getUsersWithCallback() {
      fetchUsersCallback(function(userList) {
        renderUsers(userList);
      });
    }

    //  Promise
    function fetchUsersPromise() {
      return new Promise((resolve) => {
        setTimeout(() => {
          resolve(users);
        }, 500);
      });
    }

    function getUsersWithPromise() {
      fetchUsersPromise().then(renderUsers);
    }

    // async/await
    async function fetchUsersAsync() {
      return new Promise((resolve) => {
        setTimeout(() => {
          resolve(users);
        }, 500);
      });
    }

    async function getUsersWithAsyncAwait() {
      const userList = await fetchUsersAsync();
      renderUsers(userList);
    }


    function renderUsers(userList) {
      const ul = document.getElementById("userList");
      ul.innerHTML = "";
      userList.forEach(user => {
        const li = document.createElement("li");
        li.textContent = `${user.name} - ${user.email}`;
        ul.appendChild(li);
      });
    }
