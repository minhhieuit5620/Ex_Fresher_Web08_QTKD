/* eslint-disable */ 
import baseApi from "./baseApi";
import common from "./common";
import Resource from "./resource";
import urlApi from "./urlApi";
const loadURL=Resource.URL.auth
export default {

   /********************
   * tạo mới token
   * HMHieu 04-11-2022
   * 
   */
   async refreshToken(accessToken,refreshToken){      
        let method=Resource.method.POST;
        let url=urlApi.apiAuth.refreshToken;
        var res= await baseApi.fetchAPIBody({accessToken,refreshToken},url,method)       
        localStorage.setItem(Resource.Manager.token.accessToken,res.data.accessToken);
        localStorage.setItem(Resource.Manager.token.refreshToken,res.data.refreshToken);               
    },


    /********************
    * revoke dữ liệu refresh token
    * HMHieu 24-11-2022
    * 
    */
   async revoke(){
           //decode token để xác định exp
          try {           
            let username=localStorage.getItem(Resource.Manager.token.username);
            if(!common.checkEmptyData(username)){
              let method=Resource.method.POST
              var res= await baseApi.fetchAPIDefault( loadURL+`Auth/revoke/${username}`,method);
              if(res.success){
                 console.log(res.success);
                 return res
              }
              
            }                       
          } catch (error) {
            console.log(error)
          }            
    }
}