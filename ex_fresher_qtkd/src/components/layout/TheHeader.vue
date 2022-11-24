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
                <div class="header__account--info"  >
                    <div class="info__img icon icon__user" @click="openToast"></div>
                   
                    <div class="info__text"  @click="openLogOut">Hứa Minh Hiếu</div>
                </div>
                <div class="header__account--toggle icon icon__drop" @click="openLogOut" ></div>
              
            </div>
        </div> 
        <button class="btn logout" v-show="showLogout" @click="Logout"> Đăng xuất</button>
        <!-- <transition name="toast-message">
            <MToastMessage v-show='isShowToast' :content = 'contentOfToastMessage' 
            @closeToastMessage='closeToastMessage' :class='{"toast-waring": isShowToast}' />
        </transition> -->
        <transition name="toast-message">
            <MToastMessage v-show='isShowToast' :content='contentOfToastMessage' @closeToastMessage='closeToastMessage'
                :class='{ "toast-waring": isShowToast}'  />
        </transition>
</template>
<script>
 /* eslint-disable */
    import auth from '@/js/common/auth';
    import common from '@/js/common/common';
    import resource from '@/js/common/resource';
    import MToastMessage from '../../components/base/MToastMsg.vue';
export default {
    components:{MToastMessage},
    data() {
        return{
            isShowToast: false,
            contentOfToastMessage: '', //nội dung toast message
            showLogout:false,
        }
    },
    methods:{
       
        /********************
        * open toast đang thi công
        * HMHieu 24-11-2022
        * 
        */
        openToast(){ 
            let me=this;
            me.contentOfToastMessage = resource.ToastMessage.notComplete ;
            me.isShowToast = true;          
            setTimeout(function(){
                me.isShowToast = false;
            }, 1500);
        },
        //đóng toast đang thi công
        closeToastMessage(){
            this.isShowToast = false;
        },

        /********************
        * mở form đăng xuất
        * HMHieu 24-11-2022
        * 
        */
        openLogOut(){
            this.showLogout= !this.showLogout;
        },

        /********************
        * đăng xuất khỏi chương trình
        * HMHieu 24-11-2022
        * 
        */
       async Logout(){         
            var accessToken=localStorage.getItem("token");

            //revoke refresh token
            if(!common.checkEmptyData(accessToken)){              
                await auth.revoke();             
            }
            localStorage.clear();
            this.$router.push("/login");
           
        }
    }
}
</script>
<style scoped>
/* .header__account{
    position: absolute;
} */
.logout{
    position: absolute;
    top: 50px;
    right: 40px;
    z-index: 10;
    background-color: #fff;
    color: #000;
   
    border: 1px solid #ccc;
}
</style>
