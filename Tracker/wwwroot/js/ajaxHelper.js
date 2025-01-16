// Centralized AJAX configuration
const AjaxHelper = {
  baseUrl: "https://localhost:7096/api/",

  // Get auth token from cookie
  getCookie: function (name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(";").shift();
  },

  request: function (endpoint, method, data = null) {
    const config = {
      url: this.baseUrl + endpoint,
      type: method,
      headers: {
        Authorization: "Bearer " + this.getCookie("AuthToken"),
      },
      contentType: "application/json",
    };

    if (data) {
      config.data = JSON.stringify(data);
    }

    return $.ajax(config);
  },

  get: function (endpoint) {
    return this.request(endpoint, "GET");
  },

  post: function (endpoint, data) {
    return this.request(endpoint, "POST", data);
  },

  put: function (endpoint, data) {
    return this.request(endpoint, "PUT", data);
  },

  delete: function (endpoint) {
    return this.request(endpoint, "DELETE");
  },

  handleResponse: function (
    response,
    successCallback,
    errorMessage = "An error occurred."
  ) {
    if (response.isSuccess) {
      successCallback(response.data);
    } else {
      alert(response.message || errorMessage);
    }
  },
};
