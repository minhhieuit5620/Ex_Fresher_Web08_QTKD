 /* eslint-disable */
import { createRouter, createWebHashHistory } from "vue-router";

import employeeList from'../pages/employee/EmployeeList.vue'
import ImportData from'../pages/employee/ImportData.vue'

import byePage from'../pages/otherPages/BuyPage.vue'
import ccdcPage from'../pages/otherPages/CcdcPage.vue'
import moneyBank from'../pages/otherPages/MoneyBank.vue'
import moneyPage from'../pages/otherPages/MoneyPage.vue'
import orderPage from'../pages/otherPages/OrderPage.vue'
import salePage from'../pages/otherPages/SalePage.vue'
import taxPage from'../pages/otherPages/TaxPage.vue'
import warePage from'../pages/otherPages/WarePage.vue'
import ErrorPage from '../pages/otherPages/ErrorPage.vue';
import ManagerUser from '../pages/otherPages/ManagerUser.vue';
import loginUser from'../pages/LoginAmis.vue';
import registerUser from'../pages/RegisterAmis.vue';
import assetPage from'../pages/otherPages/AssetPage.vue';
import Resource from "@/js/common/resource";

// import store from '@/store'; // this-is: dùng để lấy dữ liệu từ Vuex.

const logged=true;
var backLogin=false;

const routers=[
    {
        path:"/login",
        component:loginUser,
        meta:{
            notBackLogin:true
        }  
     
    },
    {
        path:"/register",
        component:registerUser,
        meta:{
            notBackLogin:true
        }  
      
    },
    {path:"/:pathMatch(.*)*",component:ErrorPage},  
    {
        path:"/",
        component:employeeList,
        meta:{
            needAuth:true
        }        
    },

    {path:"/employeeList",
        component:employeeList,
        meta:{
            needAuth:true
        }       
    },
    {
        path:"/managerUser",
        component:ManagerUser,
        meta:{
            needAuth:true
        }

    },
    {
        path:"/import",
        component:ImportData,
        meta:{
            needAuth:true
        }

    },
    {
        path:"/assets",
        component:assetPage,       
    },
    
    {path:"/buy",component:byePage},
    {path:"/ccdcPage",component:ccdcPage},
    {path:"/moneyBank",component:moneyBank},
    {path:"/orderPage",component:orderPage},
    {path:"/salePage",component:salePage},
    {path:"/taxPage",component:taxPage},
    {path:"/warePage",component:warePage},
   
]


const router=createRouter({
    history:createWebHashHistory(),
    routes:routers,    linkActiveClass: 'active-link',
    linkExactActiveClass: 'exact-active-link',
})
router.beforeEach((to,from,next)=>{ 

    //kiểm tra nếu chưa đăng nhập thì cần đăng nhập rồi mới có thể vào ứng dụng
    if(to.meta.needAuth){             
        if(logged){
            var token= localStorage.getItem(Resource.Manager.token.accessToken);           
            if(token === '' || token === undefined || token === null||token === "undefined"){
                next("/login");
                backLogin=false;

            }else{
                next();
                backLogin=true;
            }
         
        }else{
            next();
        }
    } 
    //nếu đã đăng nhập rồi thì không cho quay lại đăng nhập và đăng ký
    else if(to.meta.notBackLogin){
        console.log(backLogin);
        if(backLogin===true){                   
            var token= localStorage.getItem(Resource.Manager.token.accessToken);           
            if(token === '' || token === undefined || token === null||token === "undefined"){
                next("/login");
                backLogin=false;
            }else{
                next("/");      
                backLogin=true;       
            }            
         }else{
            next();  
        }
    }

    else{                 
        next();    
    }


    // if(to.meta.notBackLogin){
    //     console.log(backLogin);
    //     if(backLogin===true){        
    //         debugger     
    //         var token= localStorage.getItem(Resource.Manager.token.accessToken);           
    //         if(token === '' || token === undefined || token === null||token === "undefined"){
    //             next("/login");
    //         }else{
    //             next("/");             
    //         }            
    //     }else{
    //         next("/");  
    //     }
    // }else{             
    //     next("/");        
    // }  
  

   
})
// const router = createRouter({
//   history: createWebHashHistory(process.env.BASE_URL),
//   routes,
// });

export default router;