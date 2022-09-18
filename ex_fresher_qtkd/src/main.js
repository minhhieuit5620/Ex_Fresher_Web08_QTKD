import { createApp } from 'vue'
import App from './App.vue'
import mcombobox from 'ms-combobox'
//b1:Khai báo 
import {createRouter,createWebHistory} from 'vue-router'

import employeeList from './pages/employee/employeeList.vue'

//B2: định nghĩa các router
const routers=[
    {path:"/",component:employeeList},
    {path:"/employeeList",component:employeeList}
]

//B3: Khởi tạo router
const router=createRouter({
    history:createWebHistory(),
    routes:routers
})
// createApp(App).mount('#app')
const app= createApp(App)
app.component('hCombobox',mcombobox)
app.use(router)
app.mount('#app')
