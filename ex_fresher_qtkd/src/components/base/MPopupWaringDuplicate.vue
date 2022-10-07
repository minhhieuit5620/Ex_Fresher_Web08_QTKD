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
                <div class="btn btn__close" @click="agreeDuplicate">Đồng ý</div>
            </div>
        </div>
    </div>

</template>
<script>
export default {
    props: {
      idEmployee:String,
      contentDuplicate: String,
      listSelectedEmployee: Object,
      popUpMode:Number,
    },
    data() {
        return{
           
        }
    },
    methods:{
        agreeDuplicate(){
            this.$emit('closeErrorDuplicate');
        }
      
    }
}
</script>