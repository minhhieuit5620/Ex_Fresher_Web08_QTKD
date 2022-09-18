<template>
     <div class="header">
            <div class="header__left">
                <div class="header__logo">
                    <div class="header__logo--icon icon icon__logo" >
                     
                    </div>
                    <router-link to="/" class="header__logo--img">
                        <img src="../../assets/img/Logo_Module_TiengViet_White.66947422.svg" alt="">
                    </router-link>
                 
                </div>
                <div class="header__select">
                    <div class="header__select--icon icon icon__toggle">
                        
                    </div>
                    <div class="header__select--cbl">
                        <hCombobox  :url="'https://cukcuk.manhnv.net/api/v1/CustomerGroups'" propValue="CustomerGroupId" propText="CustomerGroupName" >
                        </hCombobox>
                    </div>
                </div>
            </div>
           
            <div class="header__account">
                <div class="header__account--ring icon icon__ring"></div>
                <div class="header__account--info" >
                    <div class="info__img icon icon__user" ></div>
                    <div class="info__text">Hứa Minh Hiếu</div>
                </div>
                <div class="header__account--toggle icon icon__drop" ></div>
            </div>
        </div> 
</template>
<script>
export default {
    data() {
        return{}
    },
}
</script>
