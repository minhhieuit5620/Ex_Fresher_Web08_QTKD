<template>
     <!-- Detail Employee  -->
     <div class="dialog__wrap"  >       
          <div class="dialog__infoEmp" >
            <div class="dialog__infoEmp--top">
                <div class="dialog__info">
                    <div class="dialog__info--name">
                        Thông tin nhân viên
                    </div>
                    <div class="dialog__info--checkbox">
                        <div class="checkbox__item">
                            <input type="checkbox" id="customer" class="checkbox__detail input__checkbox customer " 
                           
                            >
                            <label for="customer">Là khách hàng</label>
                        </div>
                        <div class="checkbox__item">
                            <input type="checkbox" id="supplier" class="checkbox__detail input__checkbox supplier" >
                            <label for="supplier">Là nhà cung cấp</label>
                        </div>
                    </div>
                </div>
                <div class="dialog__top--right">
                    <div class="dialog__question icon icon__question" ></div>
                    <div class="dialog__close icon icon__close"  @click="closeEmpDetail"></div>
                </div>
            </div>
            <div class="dialog__infoEmp--content">
                <div class="content__info">
                    <div class="content__info--left">
                        <div class="info__left--first">
                            <div class="info__item info__code">
                                <div class="lable-input">
                                    Mã
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input">
                                    <input type="text" class="input" 
                                    v-model="emp.employeeCode"
                                    :ref="'employeeCode'" >
                                </div>
                               
                            </div>
                            <div class="info__item info__name">
                                <div class="lable-input">
                                    Tên
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input">
                                    <input type="text" class="input" v-model="emp.employeeName">
                                </div>
                               
                            </div>
                        </div>
                       
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Đơn vị
                                <span class="color-red">*</span>
                            </div>
                            <div class="content__input">
                                <hCombobox  :url="'http://localhost:3000/department'" propValue="departmentID" propText="departmentName" v-model="emp.departmentID" >
                                </hCombobox>
                            </div>
                           
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Chức danh
                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text"  v-model="emp.departmentName">
                            </div>
                           
                        </div>
                      
                    </div>
                    <div class="content__info--right">
                        <div class="info__right--first">
                            <div class="info__item ">
                                <div class="lable-input">
                                   Ngày sinh
                                    
                                </div>
                                <div class="content__input">
                                    <input class="input input__date" type="date" v-model="emp.dateOfBirth" :max="maxDate" >
                                </div>
                               
                            </div>
                            <div class="info__item item__radio">
                                <div class="lable-input">
                                  Giới tính
                                    
                                </div>
                                <div class="content__radio">
                                    <div class="radio__item">
                                        <input type="radio" class="radio__boy"  name="gender" checked>
                                        <label for="radio1">Nam</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="radio__girl" name="gender" >
                                        <label for="radio2">Nữ</label>
                                    </div><div class="radio__item">
                                        <input type="radio" class="radio__other" name="gender" >
                                        <label for="radio3">Khác</label>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                        <div class="info__right--CMND">
                            <div class="info__item item__CMND">
                                <div class="lable-input">
                                 Số CMND                                    
                                </div>
                                <div class="content__input">
                                    <input class="input" type="text" v-model="emp.identityNumber" >
                                </div>                               
                            </div>
                            <div class="info__item ">
                                <div class="lable-input">
                                  Ngày cấp                                    
                                </div>
                                <div class="content__input">
                                    <input class="input input__date" type="date" v-model="emp.identityIssuedDate" :max="maxDate" >
                                </div>                             
                            </div>
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Nơi cấp                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text" v-model="emp.identityIssuedPlace" >
                            </div>                           
                        </div>                       
                    </div>
                </div>
                <div class="content__address">
                    <div class="content__address--first">
                        <div class="info__item address__item--max">
                            <div class="lable-input">
                               Địa chỉ                                
                            </div>
                            <div class="content__input">
                                <input type="text" class="input" v-model="emp.address">
                            </div>
                           
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input">
                               ĐT di động                             
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.phoneNumbermobile">
                             </div>
                        </div>
                        <div class=" info__item item__other">
                            <div class="lable-input">
                                ĐT cố định                               
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.phoneNumberlandline">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Email                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.email">
                             </div>
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tài khoản ngân hàng                         
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.accountBank">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tên ngân hàng                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.nameBank">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Chi nhánh                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" v-model="emp.branchBank">
                             </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="dialog__infoEmp--bottom">
                <div class="dialog__left">
                    <div class=" btn btn__cancel" @click="closeEmpDetail" >
                        Hủy
                    </div>
                </div>
                <div class="dialog__right">
                    <div class="btn btn__save">
                        Cất
                    </div>
                    <div class="btn btn__saveAdd"  @click="btnSaveOnClick" >Cất và thêm</div>
                </div>
            </div>
        </div> 
    </div>
       <!-- End detail Employee -->
</template>
<script>
    import MCombobox from '@/components/base/MCombobox.vue';
    import eNum from '@/js/common/eNum';
    import common from '@/js/common/common'
export default {
    component:{MCombobox},
    name:"employeeDetail",
    props:{
        closeDialog: Function,
        employeeSelected:Function,
        formMode:Number,
    },
    created(){       
        this.emp = this.employeeSelected;      
    },
    mounted(){
        this.$refs['employeeCode'].focus();//focus vào item đầu tiên
        this.formatDate();//thay đổi dữ liệu ngày tháng theo format trước khi hiển thị 
    },
    data() {
        return{
            showForm:true,     
            emp:{},
            maxDate:common.maxDate(),               
        }
    },
    
    methods:{
          /**
         * Đóng form dialog 
         * Author: HMH(16/09/22)
        */
        closeEmpDetail(){
            this.closeDialog();
        },
        /**
         * format ngày tháng theo định dạng mong muốn
         * Author: HMH(19/09/22)
         */
        formatDate(){
            if(this.formMode==eNum.formMode.Edit){
                this.emp.dateOfBirth=common.formatDate(this.emp.dateOfBirth);
                this.emp.identityIssuedDate=common.formatDate(this.emp.identityIssuedDate);
            }
        },
         /**
         * Thêm mới hoặc sửa dữ liệu theo formMode
         * Author: HMH(19/09/22)
         */
        btnSaveOnClick() {
        var me = this;
        var method = "POST";
        var url = "http://localhost:3000/employee";
        // validate dữ liệu:

        // Cất dữ liệu:

        if (me.formMode == 2) {
            method = "PUT";
            url = url + `/${me.emp.id}`;
        }       
        fetch(url, {
            
            method: method,
            headers: {
            "Content-Type": "application/json",
            },
            body: JSON.stringify(me.emp),
        })
            .then((res) => res.json())
            .then((res) => {
            var status = res.status;
                 me.$emit("closeDialog");    
                 console.log(status);
            })
            .catch((res) => {
            console.log(res);
            
            });
        },
    },
  
 

}
</script>