<template>
    <div class="wrap__waring" >
        <div class="dialog__waring">
            <div class="dialog__waring--top"></div>
            <div class="dialog__waring--content">
                <div class="waring__icon icon icon__waring"></div>
                <div class="waring__content">
                    <Span>{{content}}</Span>
                </div>
            </div>
            <div class="dialog__two--bottom">
                <div class="btn btn__cancel--delete" @click="cancelDelete()">Không</div>
                <div class="btn btn__aprove" @click="aproveDelete()">Có</div>
            </div>
        </div>
    </div>

</template>
<script>
export default {
    props: {
      idEmployee:String,
      content: String
    },
    data() {
        return{
           
        }
    },
    methods:{
        cancelDelete(){
            this.$emit('closePopup');
        },
        aproveDelete(){
            this.$emit('employeeDelete', this.idEmployee)
        }
    }
}
</script>