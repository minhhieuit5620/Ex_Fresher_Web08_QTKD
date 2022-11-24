/* eslint-disable */
import { createApp } from 'vue'
import App from './App.vue'
import mcombobox from 'ms-combobox'

import combox from'./components/base/MCombobox.vue'

//datepicker
import Datepicker from '@vuepic/vue-datepicker';


import DragItDude from 'vue-drag-it-dude'
import '@vuepic/vue-datepicker/dist/main.css'

import router from "./router";


const app= createApp(App);


app.component('datePicker',Datepicker)
app.component('vue-drag-it-dude', DragItDude)

app.component('hCombobox',mcombobox)
app.component('MCombobox',combox)

app.use(router)
app.mount('#app')
