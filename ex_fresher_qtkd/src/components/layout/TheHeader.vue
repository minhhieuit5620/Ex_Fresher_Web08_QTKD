<template>
     <div class="header">
            <div class="header__left">
                <div class="header__logo">
                    <div class="header__logo--icon icon icon__logo" >
                     
                    </div>
                    <router-link to="avbc" class="header__logo--img">
                        <img src="../../assets/img/Logo_Module_TiengViet_White.66947422.svg" alt="">
                    </router-link>
                 
                </div>
                <div class="header__select">
                    <div class="header__select--icon icon icon__toggle">
                        
                    </div>
                    <div class="header__select--cbl">
                        <div class="option-company" @click="openToast">CÔNG TY TNHH SẢN XUẤT-THƯƠNG MẠI-DỊCH VỤ-QUI PHÚC</div>
                        <div class="arrow-down-icon" @click="openToast"></div>
                    </div>
                </div>
            </div>
           
            <div class="header__account">
                <div class="tooltip">
                 <div class="header__account--ring icon icon__ring" @click="openToast"></div>
                 <span class="tooltiptext tooltipIcon">Thông báo</span>
                </div>
                <div class="header__account--info" @click="openToast" >
                    <div class="info__img icon icon__user"></div>
                   
                    <div class="info__text">Hứa Minh Hiếu</div>
                </div>
                <div class="header__account--toggle icon icon__drop" @click="openToast" ></div>
            </div>
        </div> 
        <transition name="toast-message">
            <MToastMessage v-show='isShowToast' :content = 'contentOfToastMessage' 
            @closeToastMessage='closeToastMessage' :class='{"toast-waring": isShowToast}' />
        </transition>
</template>
<script>
    import resource from '@/js/common/resource';
    import MToastMessage from '../../components/base/MToastMsg.vue';
export default {
    components:{MToastMessage},
    data() {
        return{
            isShowToast: false,
            contentOfToastMessage: '', //nội dung toast message
        }
    },
    methods:{
       
        openToast(){ 
            let me=this;
            me.contentOfToastMessage = resource.ToastMessage.notComplete ;
            me.isShowToast = true;          
            setTimeout(function(){
                me.isShowToast = false;
            }, 2000);
        },
        closeToastMessage(){
            this.isShowToast = false;
        },
    }
}
</script>
