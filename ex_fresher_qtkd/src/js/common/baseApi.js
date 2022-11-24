// // import eventBus from './eventVue.js';
// import eNum from './eNum';
// import Resource from './resource';
//export default EventBus;
// import Vue from 'vue';
// Vue.prototype.$eventBus = new Vue();

import auth from "./auth";
import eNum from "./eNum";
import Resource from "./resource";

/* eslint-disable */
export default {
  /**
   * Base gọi API getData
   * @param {*} urlAPI
   * @returns
   * createdBy: HMHieu(24-10-2022)
   */
  async fetchAPIDefault(urlAPI, method) {
    // debugger
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    var resfreshToken = localStorage.getItem(
      Resource.Manager.token.refreshToken
    );
    if (token !== null && token !== "" && token !== "undefined") {
      var response = await fetch(urlAPI, {
        method: method,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });
      let status = response.status;

      if (status === eNum.errorBackEnd.Login) {
        await auth.refreshToken(token, resfreshToken);
        token = localStorage.getItem(Resource.Manager.token.accessToken);
        response = await fetch(urlAPI, {
          method: method,
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        });
      }
      const responseJSON = await response.json();

      return responseJSON;
    } else {
      localStorage.clear();
    }
  },

   /**
   * Base gọi API getData
   * @param {*} urlAPI
   * @returns
   * createdBy: HMHieu(24-10-2022)
   */
    async fetchAPIDefaultV2(urlAPI, method) {
        // debugger
        var token = localStorage.getItem(Resource.Manager.token.accessToken);
        var resfreshToken = localStorage.getItem(
          Resource.Manager.token.refreshToken
        );
        if (token !== null && token !== "" && token !== "undefined") {
          var response = await fetch(urlAPI, {
            method: method,
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          });
          let status = response.status;
    
          if (status === eNum.errorBackEnd.Login) {
            await auth.refreshToken(token, resfreshToken);
            token = localStorage.getItem(Resource.Manager.token.accessToken);
            response = await fetch(urlAPI, {
              method: method,
              headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
              },
            });
          }
          
    
          return response;
        } else {
          localStorage.clear();
        }
      },

  /**
   * Decode token trả về để xác định role
   * @param { } token
   * @returns role
   */
  parseJwt(token) {
    if (token !== null && token !== "" && token !== "undefined") {
      var base64Url = token.split(".")[1];
      var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      var jsonPayload = decodeURIComponent(
        window
          .atob(base64)
          .split("")
          .map(function (c) {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );
      var role = JSON.parse(jsonPayload);

      for (let key in role) {
        if (key.includes("role")) {
          return role[key];
        }
      }

      return "User";
    }
  },

  /**
   * Base gọi API thêm mới, sửa hoặc nhân bản dữ liệu
   * @param {*} data
   * @param {*} urlAPI
   * @returns
   * createdBy: HMHieu(24-10-2022)
   */
  async fetchAPIBody(data, urlAPI, method) {
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    var resfreshToken = localStorage.getItem(Resource.Manager.token.refreshToken);          

    let response = await this.bodyFetch(urlAPI, data, method);

    let status = response.status;

    if (status === eNum.errorBackEnd.Login) {
      await auth.refreshToken(token, resfreshToken);
      response = await this.bodyFetch(urlAPI, data, method);
    }

    const responseJSON = await response.json();
    return responseJSON;
    // return response
  },

  /**
   * Base gọi API thêm mới, sửa hoặc nhân bản dữ liệu
   * @param {*} data
   * @param {*} urlAPI
   * @returns
   * createdBy: HMHieu(24-10-2022)
   */
  async fetchAPIBodyV2(data, urlAPI, method) {
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    var resfreshToken = localStorage.getItem(
      Resource.Manager.token.refreshToken
    );

    let response = await this.bodyFetch(urlAPI, data, method);

    let status = response.status;

    if (status === eNum.errorBackEnd.Login) {
      await auth.refreshToken(token, resfreshToken);
      response = await this.bodyFetch(urlAPI, data, method);
    }

    return response;
    // return response
  },

  /**
   * Base gọi API thao tác với file
   * @param {*} urlAPI
   * @param {*} file
   * @param {*} method
   * @returns
   */
  async fetchAPIFile(urlAPI, file, method) {
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    var resfreshToken = localStorage.getItem(
      Resource.Manager.token.refreshToken
    );

    let response = await this.bodyFetchFile(urlAPI, file, method);
    let status = response.status;
    if (status === eNum.errorBackEnd.Login) {
      await auth.refreshToken(token, resfreshToken);

      response = await this.bodyFetchFile(urlAPI, file, method);
    }

    //const responseJSON = await response.json();
    return response;
  },

  /**
   * Base gọi API login
   * @param {*} data
   * @param {*} urlAPI
   * @returns
   * createdBy: HMHieu(24-10-2022)
   */
  async fetchAPILogin(data, urlAPI, method) {
    // debugger
    //let response=
    let response = await fetch(urlAPI, {
      method: method,
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    //await this.bodyFetch(urlAPI,data,method)
    return response;
  },

  /* fetch body call API
   * Hàm xử lý body truyền vào khi call API thêm, sửa,.. liên quan đến body
   * createdBy: HMHieu(24-10-2022)
   */
  async bodyFetch(urlAPI, data, method) {
    debugger;
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    if (token !== null && token !== "" && token !== "undefined") {
      const response = await fetch(urlAPI, {
        method: method,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(data),
      });
      return response;
    } else {
      localStorage.clear();
    }
  },

  /********************
   * fetch body File
   * HMHieu 15-11-2022
   *
   */
  async bodyFetchFile(urlAPI, file, method) {
    var token = localStorage.getItem(Resource.Manager.token.accessToken);
    if (token !== null && token !== "" && token !== "undefined") {
      const response = await fetch(urlAPI, {
        method: method,
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: file,
      });
      return await response;
    }
  },
};
