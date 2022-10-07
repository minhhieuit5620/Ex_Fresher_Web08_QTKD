import { createApp } from 'vue'
import App from './App.vue'
import mcombobox from 'ms-combobox'
//b1:Khai báo 
import {createRouter,createWebHistory} from 'vue-router'

import employeeList from './pages/employee/employeeList.vue'
import combox from'./components/base/MCombobox.vue'
// import assetsPage from'./pages/otherPages/assetPage.vue'
import byePage from'./pages/otherPages/buyPage.vue'
import ccdcPage from'./pages/otherPages/ccdcPage.vue'
import moneyBank from'./pages/otherPages/moneyBank.vue'
import moneyPage from'./pages/otherPages/moneyPage.vue'
import orderPage from'./pages/otherPages/orderPage.vue'
import salePage from'./pages/otherPages/salePage.vue'
import taxPage from'./pages/otherPages/taxPage.vue'
import warePage from'./pages/otherPages/warePage.vue'
//datepicker
import Datepicker from '@vuepic/vue-datepicker';


import DragItDude from 'vue-drag-it-dude'
import '@vuepic/vue-datepicker/dist/main.css'
//B2: định nghĩa các router
const routers=[
    {path:"/",component:employeeList},
    {path:"/employeeList",component:employeeList},
    {path:"/assets",component:employeeList},
    {path:"/buy",component:byePage},
    {path:"/ccdcPage",component:ccdcPage},
    {path:"/moneyBank",component:moneyBank},
    {path:"/orderPage",component:orderPage},
    {path:"/salePage",component:salePage},
    {path:"/taxPage",component:taxPage},
    {path:"/warePage",component:warePage},
    {path:"/moneyPage",component:moneyPage},
]

//B3: Khởi tạo router
const router=createRouter({
    history:createWebHistory(),
    routes:routers,    linkActiveClass: 'active-link',
    linkExactActiveClass: 'exact-active-link',
})
// createApp(App).mount('#app')
const app= createApp(App)

app.component('datePicker',Datepicker)
app.component('vue-drag-it-dude', DragItDude)

app.component('hCombobox',mcombobox)
app.component('MCombobox',combox)
app.use(router)
app.mount('#app')
