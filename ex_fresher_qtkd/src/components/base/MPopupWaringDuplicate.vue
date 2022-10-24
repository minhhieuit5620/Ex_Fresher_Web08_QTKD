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
export default {
    props: {
     
      contentDuplicate: String,
    //   listSelectedEmployee: Object,
     // popUpMode:Number,
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
            this.$emit('closeErrorDuplicate');
        }
      
    }
}
</script>