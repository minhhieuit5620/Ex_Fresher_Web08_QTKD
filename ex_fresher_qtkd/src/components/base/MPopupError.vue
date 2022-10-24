<template>
  <div class="wrap__error"  >
        <div class="dialog__error">
            <div class="dialog__error--top"></div>
            <div class="dialog__waring--content">
                <div class="error__icon icon icon__error" ></div>
                <div class="error__content">
                    <Span>{{text}}</Span>
                </div>
            </div>
            <div class="dialog__waring--bottom">
                <button class="btn btn__close" :ref="'accept'" @click="close()">Đóng</button>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    props:{
        text: String
    },
    data() {
        return{

        }
    },
    mounted(){
        this.$refs["accept"].focus(); //focus vào item đầu tiên 
    },
    methods:{
            close(){
                this.$emit('closeError');
            }
    }
}
</script>