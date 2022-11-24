<template>
     <div id="amis-login" style="">
      <div class="limiter">
        <div class="container-login" view="login" bg="4">
          <div class="wrap-login">
            <div class="login-form validate-form">
              <span class="login-form-logo"></span>
              <span
                objname="jTitle"
                class="login-form-title"
                res-key="FormLogin_Title"
                style="display: none"
                >Đăng nhập MISA AMIS</span
              >
              <span
                objname="jSubTitle"
                class="login-form-subtitle"
               
              ></span>
              <div class="wrap-input username-wrap validate-input">
                <input
                  objname="jUserName"
                  class="input ap-lg-input"
                  type="text"
                  maxlength="20"
                  name="username"
                  v-model.trim="username"
                  @input="validateEmptyUser"                 
                  placeholder-key="FormLogin_UserPlaceholder"
                  placeholder="Số điện thoại/email"
                  :class="{'border-red': emptyUserName}" 
                  @click="emptyUserName=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"
                  @focus="emptyUserName=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"

                />
                
                <span class="error-info" res-key="FormLogin_UserNotEmpty"
                v-show="emptyUserName"
                  >Tên đăng nhập không được để trống</span
                >
              </div>
              <div class="wrap-input pass-wrap validate-input">
                <input
                  objname="jPassword"
                  class="input ap-lg-input"
                  :type="typePass"
                  name="pass"
                  v-model.trim="password"
                  @input="validateEmptyPass"                 
                  placeholder-key="FormLogin_PasswordPlaceholder"
                  maxlength="20"
                  placeholder="Mật khẩu"
                  :class="{'border-red': emptyPassWord}" 
                  @click="emptyPassWord=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"
                  @focus="emptyPassWord=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"              
                />
                <div  class="icon icon__eye--show btn-show-pass" v-show="this.typePass==='text'" @click="showPassWord"> </div>
                <div  class="icon icon__eye--hide btn-show-pass"    v-show="this.typePass==='password'" @click="showPassWord"> </div>
                <span class="error-info" res-key="FormLogin_PasswordNotEmpty" 
                v-show="errorMinLength&&!emptyPassWord" 
                  >Mật khẩu phải lớn hơn hoặc bằng 4 ký tự.</span
                >
                <span class="error-info" res-key="FormLogin_PasswordNotEmpty" 
                v-show="emptyPassWord" 
                  >Mật khẩu không được để trống</span
                >
                <i objname="jBntShowPass" class="btn-show-pass"></i>
              </div>
              <div class="wrap-input pass-wrap validate-input">
                <input
                  objname="jPassword"
                  class="input ap-lg-input"
                  :type="typePass"
                  name="confirmpass"
                  v-model.trim="confirmPassWord"                
                  @input="validateEmptyConfirmPass"
                  placeholder-key="FormLogin_PasswordPlaceholder"
                  placeholder="Xác nhận mật khẩu"
                  :class="{'border-red': emptyConfirmPass}" 
                  maxlength="20"
                  @click="emptyConfirmPass=false;duplicatePass=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"
                  @focus="emptyConfirmPass=false;duplicatePass=false;duplicateUser=false;this.errorMinLength=false;this.registerFailedByEx=false"
                />
                <div  class="icon icon__eye--show btn-show-pass" v-show="this.typePass==='text'" @click="showPassWord"> </div>
                <div  class="icon icon__eye--hide btn-show-pass"    v-show="this.typePass==='password'" @click="showPassWord"> </div>
                <span class="error-info" res-key="FormLogin_PasswordNotEmpty" 
                v-show="emptyConfirmPass" 
                  >Xác nhận mật khẩu không được để trống</span
                >
                
                <span class="error-info" res-key="FormLogin_PasswordNotEmpty" 
                v-show="duplicatePass" 
                  >Thông tin mật khẩu và xác nhận mật khẩu không trùng khớp.</span
                >
                <i objname="jBntShowPass" class="btn-show-pass"></i>
              </div>

              <!-- <div class="wrap-input pass-wrap validate-input">
                <select name="role" id="role" v-model.trim="role">
                    <option value="Admin">Admin</option>
                    <option value="SupperAdmin">SupperAdmin</option>
                    <option value="User">User</option>
                </select>
              </div> -->
              
              <div
                objname="jTextLoginFail"
                class="text-login-fail"
                res-key="FormLogin_LoginFail"
                v-show="registerFailed"
              >
                Thông tin đăng ký chưa hợp lệ
              </div>
              <div
                objname="jTextLoginFail"
                class="text-login-fail"
                res-key="FormLogin_LoginFail"
                v-show="duplicateUser"
              >
                Thông tin người dùng đã tồn tại.
              </div>
              <div
                objname="jTextLoginFail"
                class="text-login-fail"
                res-key="FormLogin_LoginFail"
                v-show="registerFailedByEx"
              >
                Có lỗi xảy ra, vui lòng liên hệ Misa.
              </div>
              <div class="text-right" style="display: flex">
                <a
                  objname="jForgot"
                  class="forgot-password"
                  href="javascript:void(0)"
                  target="_blank"
                  res-key="FormLogin_ForgotPassword"
                  >Quên mật khẩu?</a
                >
                <div style="flex: 1"></div>
              </div>
              <div class="container-login-form-btn">
                <div
                  objname="jBtnLogin"
                  class="login-form-btn"
                  res-key="FormLogin_LoginButton"
                  @click="register(username,password,confirmPassWord,role)"
                >
                  Đăng ký
                </div>
              </div>
              <div class="register-block">
                <span res-key="FormLogin_DontHaveAccount"
                  >Bạn đang sử dụng MISA AMIS?  </span
                ><router-link to="/login"
                
                  class="register-btn"
                 
                 
                  >Đăng nhập
              </router-link>
              </div>
            </div>
            <div objname="jCopyRight" class="text-center copy-right-text">
              Copyright © 2012 - 2022 MISA JSC
            </div>
            <div class="apui-notice-container" style="display: none">
              <div class="apui-notice-content">
                <div class="apui-notice-left"></div>
                <div class="apui-notice-icon"></div>
                <div class="apui-notice-des"></div>
                <div class="apui-notice-right">
                  <i class="icon icon-16 icon-close"></i>
                </div>
              </div>
              <div></div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script>
/* eslint-disable */ 
//  const loadDataURL=Resource.URL.auth;

import eNum from '@/js/common/eNum';
import common from '@/js/common/common';
 import baseApi from '@/js/common/baseApi';
import Resource from '@/js/common/resource';
import urlApi from '@/js/common/urlApi';
export default {
    data(){
        return{
            username:"",
            password:"",
            confirmPassWord:"",
            emptyUserName:false,
            emptyPassWord:false,
            emptyConfirmPass:false,
            registerFailed:false,
            duplicatePass:false,
            duplicateUser:false,
            errorMinLength:false,          
            registerFailedByEx:false,
            role:"",
            typePass:"password"
        }

    },
    methods:{

      showPassWord(){    
        this.typePass = this.typePass === "password" ? "text" : "password";
      },
      /********************
      * Đăng ký thông tin người dùng
      * HMHieu 04-11-2022
      * 
      */
        async register(username,password,confirmPassWord){
            try {

                this.validateEmptyUser();
                this.validateEmptyPass();
                this.validateMinPass();
                this.validateEmptyConfirmPass();              
                this.validateDuplicatePass();
                if(this.emptyPassWord===false && 
                this.emptyUserName===false&&
                this.emptyConfirmPass===false&& 
                this.duplicatePass===false&&
                this.duplicateUser===false&&
                this.errorMinLength===false
                ){
                    var method=Resource.method.POST;
                    let url=urlApi.apiAuth.register;
                    var response = await baseApi.fetchAPILogin({username,password,confirmPassWord},url,method)  ;// get status
                    let res=await response.json();//get data
                    console.log(res);
                    // debugger;                 
                    let status= response.status;
              
                    let success=res.success;
                    if(success){
                        this.$router.push('/login');                  
                    } else{          
                      //lỗi 400
                      debugger
                        if(status===eNum.errorBackEnd.User){    
                          let errorCode=res.data.errorCode;

                          if(errorCode===eNum.errorBackEnd.DuplicateUsername){
                            this.duplicateUser=true;
                          }                         
                          else if(errorCode===eNum.errorBackEnd.confirmPassNotMatch){

                            this.duplicatePass=true;

                          }else if(errorCode===eNum.errorBackEnd.EmptyModel){
                            this.emptyUserName = true;//trống
                            this.emptyPassWord = true;
                            this.emptyConfirmPass=true;
                          }
                          else{
                            this.registerFailed=true;
                          }
                        }
                        else{
                      //lỗi 500
                          if(status===eNum.errorBackEnd.Ser){
                            this.registerFailed=true;

                          }
                      
                        }
                    } 
                }      
            } catch (error) {
                this.registerFailedByEx=true;
                console.log(error)
            }

        },
        /**
         * validate username trống
         */
        validateEmptyUser(){
            if (common.checkEmptyData(this.username)) {
                this.emptyUserName = true;//trống
            }
            else {
                this.emptyUserName = false;//không trống
            }
        },
        /**
         * validate password trống
         */
        validateEmptyPass(){
            if (common.checkEmptyData(this.password)) {
                this.emptyPassWord = true;
            }
            else {
                this.emptyPassWord = false;
            }
        },
        /**
         * validate confirm password
         */
        validateEmptyConfirmPass(){
            if(common.checkEmptyData(this.confirmPassWord)){
                this.emptyConfirmPass=true;
            }else{
                this.emptyConfirmPass=false;
            }
        },

        /**
         * Kiểm tra sự trùng lặp của pass và confirmpass
         */
        validateDuplicatePass(){
          if(this.confirmPassWord.length>0&&this.password.length>0){
              if(this.confirmPassWord!==this.password){
                this.duplicatePass=true;
              }else{
                this.duplicatePass=false;
              }
          }
        },
          /********************
    * validate dữ liệu mật khẩu phải lớn hơn hoặc bằng 4 ký tự
    * HMHieu 20-11-2022
    * 
    */
    validateMinPass(){      
      if (!common.checkEmptyData(this.password)) {
          if(this.password.length<4){
            this.errorMinLength=true;         
          }else{
            this.errorMinLength=false;
          }
      }     
    }


    }
}
</script>


<style scoped>

html,
body {
  height: 100%;
  width: 100%;
  overflow: hidden;
  font-family: Roboto, Helvetica, Arial, sans-serif;
  font-size: 14px;
  color: #212121;
}
button {
  font-family: Roboto, Helvetica, Arial, sans-serif;
}
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
*:before,
*:after {
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}
a {
  font-size: 14px;
  line-height: 1.7;
  color: #666;
  margin: 0;
  transition: all 0.4s;
  -webkit-transition: all 0.4s;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
}
a:focus {
  outline: none !important;
}
a:hover {
  text-decoration: none;
  color: #0073e6;
}
h1,
h2,
h3,
h4,
h5,
h6 {
  margin: 0;
}
p {
  font-size: 14px;
  line-height: 1.7;
  color: #666;
  margin: 0;
}
ul,
li {
  margin: 0;
  list-style-type: none;
}
input {
  outline: none;
  border: none;
}
textarea {
  outline: none;
  border: none;
}
textarea:focus,
input:focus {
  border-color: #0073e6;
  transition: all 0.5s;
}
textarea:focus::-webkit-input-placeholder {
  color: transparent;
}
textarea:focus:-moz-placeholder {
  color: transparent;
}
textarea:focus::-moz-placeholder {
  color: transparent;
}
textarea:focus:-ms-input-placeholder {
  color: transparent;
}
input::-webkit-input-placeholder {
  color: #9ea1a5;
}
input:-moz-placeholder {
  color: #9ea1a5;
}
input::-moz-placeholder {
  color: #9ea1a5;
}
input:-ms-input-placeholder {
  color: #9ea1a5;
}
textarea::-webkit-input-placeholder {
  color: #9ea1a5;
}
textarea:-moz-placeholder {
  color: #9ea1a5;
}
textarea::-moz-placeholder {
  color: #9ea1a5;
}
textarea:-ms-input-placeholder {
  color: #9ea1a5;
}
label {
  margin: 0;
  display: block;
}
button {
  outline: none !important;
  border: none;
  background: transparent;
}
button:hover {
  cursor: pointer;
}
iframe {
  border: none !important;
}
.register-block {
  display: inline-flex;
  width: 100%;
  margin-top: 40px;
  justify-content: center;
}
.register-btn {
  font-size: 14px;
  color: #0073e6;
  line-height: 17px;
  margin-left: 8px;
  display: block;
  font-weight: 500;
  text-align: center;
  text-decoration: none;
}
.forgot-password {
  font-size: 14px;
  color: #0073e6;
  line-height: 17px;
  margin-top: 0;
  display: block;
  text-align: right;
  text-decoration: none;
  margin-bottom: 24px;
}
.limiter {
  width: 100%;
  margin: 0 auto;
}
.container-login {
  width: 100%;
  min-height: 100vh;
  display: -webkit-box;
  display: -webkit-flex;
  display: -moz-box;
  display: -ms-flexbox;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
  padding: 15px;
  background-image: url("../assets/login/bg2.jpg");
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  position: relative;
  z-index: 1;
}
.container-login[bg="2"] {
  background-image: url("../assets/login/bg2.jpg");
}
/* .container-login[bg="3"] {
  background-image: url("../images/bg3.jpg");
}
.container-login[bg="4"] {
  background-image: url("../images/bg4.jpg");
}
.container-login[bg="5"] {
  background-image: url("../images/bg5.jpg");
}
.container-login[bg="6"] {
  background-image: url("../images/bg6.jpg");
}
.container-login[bg="7"] {
  background-image: url("../images/bg7.jpg");
}
.container-login[bg="8"] {
  background-image: url("../images/bg8.jpg");
} */
.container-login::before {
  content: "";
  display: block;
  position: absolute;
  z-index: -1;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  background: linear-gradient(
    rgba(0, 30, 61, 0.6) 0%,
    rgba(0, 0, 0, 0.1) 41.42%,
    rgba(0, 0, 0, 0.3) 100%
  );
}
.wrap-login {
  width: 400px;
  border-radius: 8px;
  padding: 48px 48px 40px 48px;
  min-height: 400px;
  background: #fff;
  position: relative;
  box-shadow: 0 12px 20px rgba(0, 0, 0, 0.12);
}
.copy-right-text {
  padding-top: 14px;
  font-size: 12px;
  position: absolute;
  width: 100%;
  text-align: center;
  left: 0;
  bottom: -28px;
  color: rgba(255, 255, 255, 0.6);
}
.login-form {
  width: 100%;
}
.login-form-logo {
  font-size: 60px;
  background: url(../assets/login/icon-amis-platform2.svg) center no-repeat;
  background-size: contain;
  display: block;
  display: -moz-box;
  display: -ms-flexbox;
  width: 196px;
  height: 36px;
  margin: 0 auto;
  margin-bottom: 40px;
}
.login-form-title {
  font-size: 24px;
  font-weight: bold;
  line-height: 29px;
  margin-top: 12px;
  text-align: center;
  display: block;
  margin-bottom: 32px;
}
.wrap-input {
  width: 100%;
  position: relative;
  margin-bottom: 12px;
}
.wrap-input.pass-wrap {
  margin-bottom: 16px;
}
.input {
  font-size: 16px;
  color: #212121;
  line-height: 1.2;
  display: block;
  width: 100%;
  height: 48px;
  border: 1px solid #e0e5e9;
  background: transparent;
  padding: 14px 16px 15px 16px;
  border-radius: 3px;
}
input[disabled] {
  background-color: #f2f2f2;
  user-select: none;
}
.login-form-subtitle {
  width: 100%;
  text-align: center;
  font-size: 16px;
  margin-top: -24px;
  display: block;
  margin-bottom: 24px;
}
span.error-info {
  font-size: 12px;
  color: #ff1d1d;
  height: 20px;
  line-height: 20px;
  /* display: none; */
}
.wrap-input.error .input {
  border: 1px solid #ff1d1d;
}
.wrap-input.error span.error-info {
  display: block;
}
.btn-show-pass {
  display: block;
  position: absolute;
  right: 8px;
  top: 16px;
  /* width: 16px;
  height: 16px; */
  /* background: url("../images/icons/icon-hide-pass.svg") center no-repeat; */
}
.btn-show-pass.active {
  display: block;
  /* background: url("../images/icons/icon-show-pass.svg") center no-repeat; */
}
.login-form-btn {
  position: relative;
  width: 100%;
  font-size: 16px;
  color: #fff;
  line-height: 48px;
  font-weight: 500;
  text-align: center;
  cursor: pointer;
  height: 48px;
  border-radius: 3px;
  background: #0073e6;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
}
.login-form-btn.loading {
  text-transform: none;
}
.login-form-btn.loading:after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  color: #fff;
  /* background: url(../images/loading.gif) center no-repeat; */
  background-size: cover;
  -o-transition: all 0.4s;
  -moz-transition: all 0.4s;
  border-radius: 3px;
  opacity: 0.6;
  cursor: default;
}
.login-form-btn:hover {
  background-color: #384fd5;
  box-shadow: 0 2px 15px rgba(42, 126, 252, 0.25);
  transition: all 0.5s;
}
.login-form-btn.loading:hover {
  background-color: #0073e6;
  box-shadow: none;
}
@media (max-width: 576px) {
  .wrap-login {
    padding: 55px 15px 37px 15px;
  }
}
.text-login-fail {
  height: 24px;
  line-height: 14px;
  /* display: none; */
  color: #ff1d1d;
  padding: 0;
}
.text-login-fail.show {
  display: block;
}
#amis-tenants {
  width: 100%;
  height: 100%;
  position: fixed;
  z-index: 9999999;
  left: 0;
  top: 0;
  display: none;
  -webkit-transition: all 1s ease;
  -moz-transition: all 1s ease;
  -o-transition: all 1s ease;
  transition: all 1s ease;
  box-shadow: 0 1px 8px rgba(0, 0, 0, 0.1);
}
#amis-tenants.show {
  display: block;
}
.tenants-container {
  width: 430px;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translateX(-50%) translateY(-50%);
  background-color: #fff;
  border-radius: 3px;
  padding: 24px;
}
.tenants-container .tenants-title {
  font-size: 20px;
  margin-bottom: 30px;
  height: 30px;
  line-height: 30px;
  font-weight: 500;
}
.tenants-container .list-tenants {
  min-height: 80px;
  max-height: 200px;
  overflow-y: auto;
}
.tenants-container .tenants-footer {
  height: 48px;
  padding: 16px 0 0 0;
}
.tenants-container .tenants-footer .btn-ok {
  height: 32px;
  padding: 0 16px;
  background-color: #0073e6;
  color: #fff;
  line-height: 32px;
  text-align: center;
  border-radius: 3px;
  float: right;
  cursor: pointer;
}
.tenants-container .tenants-footer .btn-ok:hover {
  background-color: #384fd5;
}
.tenants-container .list-tenants .misa-select-field-item {
  margin: 8px 0;
}
.amis-select-type.horizontal {
  display: inline-flex;
}
.amis-select-type .misa-radio {
  display: block;
  position: relative;
  cursor: pointer;
  padding-left: 22px;
  font-size: 16px;
  height: 28px;
  line-height: 20px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  text-transform: none;
  font-weight: normal;
  color: #212121;
}
.amis-select-type .misa-radio input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  display: none;
}
.amis-select-type .checkmark {
  position: absolute;
  top: 3px;
  left: 0;
  height: 14px;
  width: 14px;
  border: 1px solid #8d9ba2;
  background-color: #fff;
  border-radius: 50%;
}
.amis-select-type .checkmark:after {
  display: none;
  content: "";
  position: absolute;
}
.amis-select-type .misa-radio .checkmark:after {
  top: 2px;
  left: 2px;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: #0073e6;
}
.amis-select-type .misa-radio input:checked ~ .checkmark {
  background-color: #fff;
  border: 1px solid #0073e6;
}
.amis-select-type .misa-radio input:checked ~ .checkmark:after {
  display: block;
}
.coming-soon-container {
  width: 500px;
  height: 500px;
  position: absolute;
  left: 50%;
  top: 50%;
  -ms-transform: translateX(-50%) translateY(-50%);
  -webkit-transform: translateX(-50%) translateY(-50%);
  -o-transform: translateX(-50%) translateY(-50%);
  -moz-transform: translateX(-50%) translateY(-50%);
  transform: translateX(-50%) translateY(-50%);
}
.coming-soon-graphic {
  background-position: center;
  background-size: contain;
  height: 330px;
  width: 100%;
  background-repeat: no-repeat;
  margin-bottom: 30px;
}
.coming-soon-title {
  font-size: 24px;
  line-height: 24px;
  font-weight: bold;
  margin-bottom: 12px;
  text-align: center;
}
.coming-soon-description {
  height: 24px;
  display: block;
  margin: 0 auto;
  text-align: center;
  background-repeat: no-repeat;
  background-size: contain;
}
.lang-container {
  position: fixed;
  top: 24px;
  right: 24px;
  width: 120px;
  height: 32px;
  z-index: 99;
  border-radius: 4px;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.1);
  line-height: 32px;
  color: #f8f8f8;
  padding: 0 16px 0 38px;
  cursor: pointer;
}
.lang-container:hover {
  border: 1px solid rgba(255, 255, 255, 0.3);
  background: rgba(255, 255, 255, 0.3);
}
.lang-container .left-icon {
  position: absolute;
  left: 8px;
  top: 7px;
  height: 16px;
  width: 21px;
}
.lang-container .selected-value {
  line-height: 32px;
}
.lang-container .icon-trigger {
  display: none;
}
.misa-dropdown-menu {
  position: absolute;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
  z-index: 9999;
  background-color: #fff;
  border-radius: 3px;
  max-height: 400px;
  padding-top: 8px;
  padding-bottom: 8px;
  overflow-x: hidden;
  overflow-y: hidden;
}
.misa-dropdown-menu:hover {
  overflow-y: auto;
}
.misa-dropdown-menu .result-item {
  height: 32px;
  line-height: 32px;
  font-size: 14px;
  color: #212121;
  padding: 0 16px 0 16px;
  width: 100%;
  display: inline-flex;
  overflow: hidden;
  white-space: nowrap;
  -ms-text-overflow: ellipsis;
  -o-text-overflow: ellipsis;
  text-overflow: ellipsis;
}
.misa-dropdown-menu .result-item .result-item-text {
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
.misa-dropdown-menu .result-item.i-bottom {
  border-bottom: 1px solid #e0e6e8;
}
.misa-dropdown-menu .result-item:last-child {
  border-bottom: none;
}
.misa-dropdown-menu .result-item:hover {
  background-color: #f2f2f2;
  cursor: pointer;
}
.misa-dropdown-menu .dropdow-title {
  height: 40px;
  line-height: 40px;
  padding-left: 16px;
  font-weight: 700;
}
/* .icon {
  background-repeat: no-repeat;
  background-color: transparent;
  background-position: 0 center;
} */
.captcha-box {
  display: flex;
}
.captcha-box input {
  width: 142px;
}
.captcha-box img {
  width: 130px;
  height: 48px;
  margin-left: 8px;
}
.captcha-box .btn-captcha-reload {
  width: 32px;
  height: 48px;
  /* background-image: url("../images/icons/icon-reload.svg"); */
  background-repeat: no-repeat;
  background-position: center;
  background-size: 24px;
  margin-left: 8px;
  cursor: pointer;
}
.apui-notice-container {
  position: fixed;
  box-shadow: 0 3px 16px rgba(0, 0, 0, 0.16);
  border: none;
  border-radius: 3px;
  z-index: 99999;
  min-width: 200px;
  max-width: 600px;
  min-height: 52px;
  overflow: hidden;
  -webkit-transition: all 0.5s ease;
  -moz-transition: all 0.5s ease;
  -o-transition: all 0.5s ease;
  transition: all 0.5s ease;
  background: #fff;
}
.apui-notice-container .apui-notice-content {
  display: flex;
  min-height: 52px;
}
.apui-notice-container.notice-center {
  left: 50%;
  -o-transform: translateX(-50%);
  -ms-transform: translateX(-50%);
  -moz-transform: translateX(-50%);
  -webkit-transform: translateX(-50%);
  transform: translateX(-50%);
}
.apui-notice-container .apui-notice-left {
  width: 4px;
  min-height: 52px;
  border-bottom-left-radius: 3px;
  border-top-left-radius: 3px;
  overflow: hidden;
}
.apui-notice-container .apui-notice-icon i {
  margin-left: 12px;
  height: 52px;
  cursor: default;
  background-repeat: no-repeat;
  background-position: center;
  display: block;
}
.apui-notice-container .apui-notice-right {
  width: 40px;
  min-height: 52px;
  border-bottom-right-radius: 3px;
  border-top-right-radius: 3px;
  position: relative;
  overflow: hidden;
}
.apui-notice-container .apui-notice-right .icon-close {
  position: absolute;
  right: 12px;
  /* background-image: url("../images/icons/ic_close_tint_16.svg"); */
  top: 16px;
  width: 16px;
  height: 16px;
}
.apui-notice-container .apui-notice-des {
  padding: 16px;
  padding-left: 8px;
  -ms-flex: 1;
  line-height: 19px;
  flex: 1;
}
.show-login-link {
  font-size: 14px;
  color: #2680eb;
  line-height: 17px;
  margin-top: 16px;
  display: block;
  text-align: center;
  text-decoration: none;
}
.otp-try-other {
  color: #0073e6;
  cursor: pointer;
  margin: 12px 0 16px 0;
}
.otp-remember-check {
  padding-top: 8px;
}
.otp-form-subtitle {
  font-size: 14px;
  line-height: 17px;
  margin-top: 8px;
  text-align: left;
  color: #666;
  display: block;
  margin-bottom: 16px;
}
.method-list {
  min-height: 188px;
}
.method-item {
  height: 60px;
  padding: 10px 10px 10px 36px;
  border-bottom: 1px solid #e0e0e0;
  background-position: 0 10px;
  background-size: 24px;
  background-repeat: no-repeat;
  cursor: pointer;
}
.method-item:hover {
  background-color: #f2f2f2;
}
.method-item:last-child {
  border-bottom: none;
}
/* .method-item[method="email"] {
  background-image: url(../images/icons/icon-email.svg);
}
.method-item[method="sms"] {
  background-image: url(../images/icons/icon-message.svg);
}
.method-item[method="app"] {
  background-image: url(../images/icons/icon-authen-secure.svg);
} */
.border-red{
  border:1px solid #ff1d1d;
}
</style>