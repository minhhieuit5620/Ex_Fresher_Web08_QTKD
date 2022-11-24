<template>
       <div class="container">
   
    
   <the-sidebar> </the-sidebar>

   <the-header></the-header>
    <div class="manager__role">
        <div class="option-company">Thêm mới vai trò</div>
        <div class="manager__role--add">
            <input class="input" v-model="nameRole" placeholder="Nhập vào tên vai trò"/>
            <div class="btn " @click="addRole()">Thêm</div>
        </div>
        
        <table class="table table__role table-hover">
            <thead>
                <tr>
                    <th>STT</th>
                    <th>Vai trò</th>
                    <th>Thao tác</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for=" (item, index) in role" :key="item.name">
                    <td>{{index++}}</td>
                    <td>{{item.name}}</td>
                    <td><div class="btn btn__delete" @click="deleteRole(item.name)">Xóa</div></td>
                </tr>
            </tbody>
        </table>
        
        <div class="option-company">Thông tin người dùng</div>
        <table class="table table__user table-hover">
            <thead>
                <tr>
                    <th>STT</th>
                    <th>UserName</th>
                    <th>Role</th>
                   
                    <th>Thao tác</th>
                    
                </tr>
            </thead>
            <tbody>
                <tr v-for=" (item, index) in user" :key="item.userName">
                    <td>{{index++}}</td>
                    <td>{{item.userName}}</td>
                    <td class="checkRole">
                        <div  v-for=" (item1) in role" :key="item1.name" >    
                                 <!-- <label for="">{{item.roleName[index1++]}}</label>        -->
                            <input type="checkbox" :id="item1.name" :checked="item1.name===item.roleName[0]" :value="item1.name"  >
                            <label :for="item1.name">{{item1.name}}</label>
                        </div>
                        <!-- {{item1.name}} -->
                    </td>
                    <td><div class="btn btn__delete" @click="deleteUser(item.userName)">Xóa</div></td>
                </tr>
            </tbody>
        </table>
        
        
    </div>
    </div>
</template>
<script>

/* eslint-disable */ 
import TheHeader from '../../components/layout/TheHeader.vue';

import TheSidebar from '../../components/layout/TheSidebar.vue';
import baseApi from '@/js/common/baseApi';
import Resource from '@/js/common/resource';
import urlApi from '@/js/common/urlApi';

const loadDataURL=Resource.URL.auth;
export default {
    components: {TheHeader,TheSidebar },
    data(){
        return{
            nameRole:"",
            role:[],
            user:[],         
        }
    },
    async created(){
       await this.getRole();
       await this.getUser();
    },
    methods:{

        /********************
        * lấy thông tin các vai trò
        * HMHieu 07-11-2022
        * 
        */
        async getRole(){
           try {
            let method=Resource.method.GET;
            let url=urlApi.apiAuth.getRole;
            // loadDataURL+"UsersManager/GetRole"
            var res=await baseApi.fetchAPIDefault(url,method);
            let success=res.success
            if(success){
                this.role=res.data;
            }else{
                console.log(res);
            }
            
           } catch (error) {
                console.log(error)
           }
        },

        /********************
        * thêm vai trò mới
        * HMHieu 07-11-2022
        * 
        */
        async addRole(){
           try {
            let method=Resource.method.POST;
            let url=urlApi.apiAddRole(this.nameRole);
            // loadDataURL+`UsersManager/AddRole?roleName=${this.nameRole}`
            var res=await baseApi.fetchAPIDefault(url,method);
            let success=res.success
            if(success){
                this.getRole();
            }else{
                console.log(res);
            }
           } catch (error) {
                console.log(error)
           }
        },
        
        /********************
        * xóa một vai trò
        * HMHieu 07-11-2022
        * 
        */
        async deleteRole(nameRole){
           try {
                let method=Resource.method.POST;
                let url=urlApi.apiDeleteRole(nameRole);
            // loadDataURL+`UsersManager/DeleteRole?idRole=${nameRole}`
                var res=await baseApi.fetchAPIDefault(url,method);
                let success=res.success
                if(success){
                    this.getRole();
                }else{
                    console.log(res);
                }
           } catch (error) {
                console.log(error)
           }
        },

        /********************
        * lấy thông tin người dùng
        * HMHieu 07-11-2022
        * 
        */
        async getUser(){
        
            // try {
                let method=Resource.method.GET
                let url=urlApi.apiAuth.getUser
                // loadDataURL+`UsersManager/GetUser`
                var res=await baseApi.fetchAPIDefault(url,method);
               
                let success=res.success
                if(success){
                    this.user=res.data;
                    console.log(this.user);
                }else{
                    console.log(this.user);
                }
            // } catch (error) {
            //     console.log(error)
            // }
        },

        /**
         * xóa thông tin người dùng
         * @param {*} username 
         */
        async deleteUser(username){
           try {
                let method=Resource.method.POST;
                let url =urlApi.apiDeleteUser(username);
                // loadDataURL+`UsersManager/DeleteUser?userName=${username}`
                var res=await baseApi.fetchAPIDefault(url,method);
                let success=res.success
                if(success){
                    this.getUser();
                }else{
                    console.log(res);
                }
           } catch (error) {
                console.log(error)
           }
        },

    }

}
</script>
<style scoped>
.manager__role{

margin: 30px;

}
.table__role{
    width: 500px;
    
}
.table__user{
    width: 1200px;
}
.input{
    width: 230px;
}
.manager__role--add{
    display: flex;
    align-items: center;
    margin-top: 16px;
    margin-bottom: 16px;
}
 .btn{
    display: inline-block;
    margin-left: 16px;
   
}
.btn__delete{
    background-color: red;
}
.option-company{
    font-size: 32px;
}
.checkRole{
    display: flex;
    justify-content: space-between;
    align-items: center;
}
</style>