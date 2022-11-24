<template>
    <div class="wrap__waring" >
        <div class="dialog__waring">
            <div class="dialog__waring--top"></div>
            <div class="dialog__waring--content">
                <div class="waring__icon icon icon__waring"></div>
                <div class="waring__content">
                    <Span>{{contentDuplicate}}</Span>
                </div>
            </div>
            <div class="dialog__waring--bottom">
                <button class="btn btn__close" :ref="'accept'" @click="agreeDuplicate">Đồng ý</button>
            </div>
        </div>
    </div>

</template>
<script>
import eNum from '@/js/common/eNum';
export default {
    props: {
     
        contentDuplicate: String,
    //   listSelectedEmployee: Object,
        popUpModeOneBtn:Number,
    },
    data() {
        return{
           
        }
    },
    mounted(){
        this.$refs["accept"].focus(); //focus vào item đầu tiên 
    },
    methods:{
        agreeDuplicate(){
            if(this.popUpModeOneBtn===eNum.popUpModeOneBtn.warning){
                this.$emit('closeErrorDuplicate');
            }
            if(this.popUpModeOneBtn===eNum.popUpModeOneBtn.logout){
                this.$emit('closeErrorDuplicate');
                this.$emit('logOutAccount');

                
            }
        }
      
    }
}
</script>