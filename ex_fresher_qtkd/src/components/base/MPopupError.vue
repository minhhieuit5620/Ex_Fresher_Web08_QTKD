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
                <div class="btn btn__close" @click="close()">Đóng</div>
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
    methods:{
            close(){
                this.$emit('closeError');
            }
    }
}
</script>