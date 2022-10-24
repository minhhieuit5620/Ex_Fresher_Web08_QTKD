<template>
     <div class="wrap__ask" >
        <div class="dialog__ask">
            <div class="dialog__ask--top"></div>
            <div class="dialog__waring--content">
                <div class="ask__icon icon icon__ask"></div>
                <div class="error__content">
                    <Span>Dữ liệu đã bị thay đổi. Bạn có muốn cất không?</Span>
                </div>
            </div>
            <div class="dialog__ask--bottom">
                <div class="dialog__left">
                    <button class="btn btn__cancel--delete" @click="hidePopUp">Hủy</button>
                </div>
                <div class="dialog__right">                    
                    <button class="btn btn__cancel--delete" :ref="'cancel'" @click="hideDialog">Không</button>
                    <button class="btn btn__aprove--ask" @click="aprove">Có</button>
                </div>
              
            </div>
        </div>
    </div>
</template>
<script>
export default{
    props:{
       
    },
    data(){
        return{}
    },
    mounted(){
        this.$refs["cancel"].focus(); //focus vào item đầu tiên 
    },
    methods:{
        /**
         * Đóng cả popup và dialog
         * CreatedBy:HMHieu(07-10-22)
         */
        hideDialog(){
            this.$emit('closeAskDialog');            
        },
         /**
         * Ẩn popup
         * CreatedBy:HMHieu(07-10-22)
         */
        hidePopUp(){
            this.$emit('closeAsk');            
        },
         /**
         * Đồng ý
         * CreatedBy:HMHieu(07-10-22)
         */
        aprove(){
            this.$emit('btnSaveOnClick');   
            this.$emit('closeAsk');     
        }

    }

}

</script>
