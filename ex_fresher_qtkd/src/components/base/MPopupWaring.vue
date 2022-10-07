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
                <div class="btn btn__aprove" @click="aproveDelete()" >Có</div>
            </div>
        </div>
    </div>

</template>
<script>
import eNum from '@/js/common/eNum';
export default {
    props: {
      idEmployee:String,
      content: String,
      listSelectedEmployee: Object,
      popUpMode:Number,
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
            if(this.popUpMode===eNum.popUpMode.DeleteMultiple){
                this.$emit('deleteMultiple',this.listSelectedEmployee);
                
            } 
            if(this.popUpMode===eNum.popUpMode.DeleteOne) {
                this.$emit('employeeDelete', this.idEmployee)
            }
          
           
        }
    }
}
</script>