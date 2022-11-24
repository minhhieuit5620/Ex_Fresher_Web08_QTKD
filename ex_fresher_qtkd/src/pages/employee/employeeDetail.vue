<template>
     <!-- Detail Employee  -->
     <div class="dialog__wrap" id="range"  > 
       
          <!-- <div class="dialog__infoEmp" @keyup="escClose($event)" @keydown="saveBtn($event)"
          @mousemove="draging($event)"  
          @mouseup="drop"  
          @mousedown="drag"  
          :ref="'headerDialog'" > -->
          <div class="dialog__infoEmp" @keyup="escClose($event)" @keydown="saveBtn($event)" draggable>
          
            <div class="dialog__infoEmp--top" >
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
                <div class="dialog__top--right ">
                    <div class="tooltip">
                        <div class="dialog__question icon icon__question" tabIndex="22" ></div>
                        <span class="tooltiptext tooltipIcon">Giúp</span>
                    </div>
                    <div class="tooltip">
                        <div class="dialog__close icon icon__close"  @click="askESC()" tabIndex="23" ></div>
                        <span class="tooltiptext tooltipIcon tooltip__right">Đóng(ESC)</span>
                    </div>
                    
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
                                <div class="content__input tooltip" >
                                    <input type="text" class="input"  maxlength="20"
                                    @keyup = 'validateEmptyCode'
                                    @blur = 'validateEmptyCode'
                                    @input = 'validateEmptyCode'
                                    :class="{'border-red' : emptyCode}"
                                    v-model="emp.employeeCode"
                                    :ref="'employeeCode'" >
                                    <div class="empty__input tooltiptext tooltipError"  v-show="emptyCode" >Mã không được để trống</div>
                                    <div class="empty__input tooltiptext tooltipError"  v-show="errorMaxCode" >Mã không được lớn hơn 20 ký tự.</div>
                                </div>
                              
                               
                            </div>
                            <div class="info__item info__name">
                                <div class="lable-input">
                                    Tên
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input tooltip">
                                    <input type="text" class="input"   maxlength="100"
                                   
                                   
                                    @input ='validateEmptyName'
                                    @blur ='validateEmptyName'
                                    :ref="'employeeNameFocus'"
                                  
                                    :class="{'border-red': emptyName}"
                                     v-model="emp.employeeName">
                                     <div class="empty__input tooltiptext tooltipError"  v-show="emptyName">Tên không được để trống</div>
                                </div>
                               
                               
                            </div>
                        </div>
                       
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Đơn vị
                                <span class="color-red">*</span>
                            </div>
                            
                            <div class="content__input tooltip" >
                            
                                 <MCombobox :url=apiGetDepartment                                
                                 :code="'departmentCode'" 
                                :text="'departmentName'" 
                                @objectItemCombobox='objectItemCombobox'
                                :valueRender='dataCombobox'
                                @input ='validateDepartment'
                                @blur="validateDepartment"
                                @comboboxFocus="validateDepartment"    
                                :key = 'keyCombobox' 
                                :emptyCombobox="emptyDepartment"                       
                                :ref="'employeeDepartmentFocus'"
                                 />

                                 <div class="empty__input tooltiptext tooltipError" v-show="emptyDepartment">Đơn vị không được để trống</div>                                                                                           
                            </div>
                           
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Chức danh
                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text"   maxlength="255" v-model="emp.positionName">
                            </div>
                           
                        </div>
                      
                    </div>
                    <div class="content__info--right">
                        <div class="info__right--first">
                            <div class="info__item ">
                                <div class="lable-input">
                                   Ngày sinh
                                    
                                </div>
                                <div class="content__input dateOfBirth tooltip" >                                  
                                     <Datepicker 
                                    class="datepickerCTM" 
                                    placeholder="DD/MM/YYYY"
                                    :enterSubmit=true
                                    :enableTimePicker="false"
                                    :tabSubmit=true                                  
                                   
                                     format="dd/MM/yyyy" 
                                    textInput 
                                    :dayNames="['T2', 'T3', 'T4', 'T5', 'T6', 'T7','CN']"
                                                                   
                                    autoApply                                    
                                    utc 
                                    locale="vi"
                                    ref="dateOfBirth"    
                                    v-model="emp.dateOfBirth"/>                 
                                 <div class="empty__input tooltiptext tooltipError"  v-show="isDateOfBirth"> Ngày sinh không hợp lệ</div>
                                    
                                    <!-- <input class="input input__date"   type="date" v-model="emp.dateOfBirth" :max="maxDate" > -->
                                </div>
                               
                            </div>
                            <div class="info__item item__radio">
                                <div class="lable-input label-gender">
                                  Giới tính
                                    
                                </div>
                                <div class="content__radio">
                                    
                                    <div class="radio__item">
                                        <div class="bo"><input type="radio" class="input__radio " checked   id="radio__boy"  value="0" name="gender"                                           
                                         v-model="chooseGender"                                       
                                         ></div>
                                        
                                        <label for="radio__boy">Nam</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio "  id="radio__girl" value="1" name="gender"                                         
                                        v-model="chooseGender"                                      
                                        >
                                        <label for="radio__girl">Nữ</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio "   id="radio__other" value="2" name="gender"                                             
                                        v-model="chooseGender"                                      
                                        >
                                        <label for="radio__other">Khác</label>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                        <div class="info__right--CMND">
                            <div class="info__item item__CMND">
                                <div class="lable-input tooltip">
                                    Số CMND  
                                  
                                        <span class="tooltiptext tooltipAronym">Số chứng minh nhân dân</span>
                                  
                                                                  
                                </div>
                                <div class="content__input tooltip">
                                    <input class="input"  
                                     maxlength="25" type="text" 
                                     v-model="emp.identityNumber"
                                     @input="validateIdentity"
                                    
                                     
                                      >
                                      <!-- <div class="empty__input tooltiptext tooltipError"  v-show="isIdentity">Số CMND không đúng</div>    -->
                                </div>  
                                                         
                            </div>
                            <div class="info__item ">
                                <div class="lable-input">
                                  Ngày cấp                                    
                                </div>
                                <div class="content__input identityIssuedDate tooltip">
                                    <!-- <datePicker v-model="emp.identityIssuedDate" :format="format" /> -->
                                    <Datepicker  class="datepickerCTM" 
                                    placeholder="DD/MM/YYYY"                                                                 
                                     format="dd/MM/yyyy" 
                                    textInput 
                                    :dayNames="['T2', 'T3', 'T4', 'T5', 'T6', 'T7','CN']"
                                    autoApply 
                                    utc 
                                    locale="vi"
                                    :enableTimePicker="false"
                                    :enterSubmit=true
                                    :tabSubmit=true
                                   
                                    :ref="'identityIssuedDate'"
                                    v-model="emp.identityIssuedDate"/>

                                 <div class="empty__input tooltiptext tooltipError"  v-show="isIdentityIssuedDate">Ngày cấp không hợp lệ</div>

                                    <!-- <input class="input input__date"  tabIndex="10" type="date" v-model="emp.identityIssuedDate" :max="maxDate" > -->
                                </div>                             
                            </div>
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Nơi cấp                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text"    maxlength="255" v-model="emp.identityIssuedPlace" >
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
                                <input type="text" class="input"  maxlength="255" v-model="emp.address">
                            </div>
                           
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input tooltip  tooltip__orther">
                               ĐT di động      
                               <span class="tooltiptext tooltipAronym">Điện thoại di động</span>                       
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  maxlength="50" 
                              
                                 v-model="emp.mobile"                               
                                 >
                                 
                             </div>
                                 
                        </div>
                        <div class=" info__item item__other">
                            <div class="lable-input tooltip tooltip__orther">
                                ĐT cố định      
                                <span class="tooltiptext tooltipAronym">Điện thoại cố định</span>                             
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"    maxlength="50" 
                                
                                 v-model="emp.landlinePhone"                               
                                 >                                
                             </div>
                             
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Email                                
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  
                               
                                 :class="{'border-red': errorEmail}"  
                                 maxlength="100" 
                                 @input="validateEmail"
                                 v-model="emp.email"
                                 :ref="'email'"
                                 >
                                 <div class="empty__input tooltiptext tooltipError"  v-show="errorEmail">Email chưa đúng định dạng</div>
                            </div>
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tài khoản ngân hàng                         
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="25" v-model="emp.bankAccount">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tên ngân hàng                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="255" v-model="emp.bankName">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Chi nhánh                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="255" v-model="emp.bankBranch">
                             </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="dialog__infoEmp--bottom">
                <!-- <div class="tooltip">
                        <div class="dialog__question icon icon__question" tabIndex="22" ></div>
                        <span class="tooltiptext tooltipIcon">Giúp</span>
                    </div> -->
                <div class="dialog__left tooltip">
                    <button class="btn btn__cancel tooltip" @click="closeEmpDetail"    @keydown="keyCancel($event)">
                        Hủy
                        <span class="tooltiptext tooltipIcon">Huỷ</span>
                    </button>                   
                </div>
                <div class="dialog__right">
                    <button class="btn btn__save tooltip"  @click="btnSaveOnClick(this.Save)"   @keydown="keySave($event,this.Save)">
                        Cất
                        <span class="tooltiptext tooltipIcon">Cất (Ctrl + S)</span>
                    </button>
                    <button class="btn btn__saveAdd tooltip" @click="btnSaveOnClick(this.SaveAndAdd)"  @keydown="keySave($event,this.SaveAndAdd)" >
                        Cất và thêm
                        <span class="tooltiptext tooltipIcon tooltip__large tooltip__right">Cất và thêm (Ctrl + Shift + S)</span>
                    </button>
                    <button @focus="this.$refs.employeeCode.focus(); "  ref="btn__SaveAdd" style="height:0;outline:0; width: 0;border: 0;padding: 0;"> </button>
                </div>
            </div>
        
        </div> 
      
    </div>
       <!-- End detail Employee -->
       <!-- <MPopupAsk />  -->
        <MPopupAsk v-show="showPopUpAsk" @btnSaveOnClick="btnSaveOnClick"  @closeAsk='closeAsk' @closeAskDialog='closeAskDialog'></MPopupAsk>
</template>
<script>
 /* eslint-disable */
     import {drag} from'@/js/common/dragDrop.js';
    // import MCombobox from '@/components/base/MCombobox.vue';
    import eNum from '@/js/common/eNum';
    import common from '@/js/common/common';
    import Resource from '@/js/common/resource';
    import MPopupAsk from '../../components/base/MPopupAsk.vue';
    import Datepicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css';

    import baseApi from '@/js/common/baseApi';
    import urlApi from '@/js/common/urlApi';
    

     /**
     * Hàm validate
     * author: HMHieu(14/9/2022)
     * @param {*} form: con trỏ vue
     * @param {object} objectEmpty {emptyCode, emptyName, emptyDepartment} 

     * 
     */
     function validate(mouse, objectEmpty){
        //validate dữ liệu bắt buộc nhập không được bỏ trống
        for(let key in objectEmpty){
            if(objectEmpty[key]){                             
                mouse.form.$emit('errorEmpty', key); 
                console.log(objectEmpty[key]);        
                return false;                
            }
        } 
        //validate email 
        let validateEmail = common.checkEmail(mouse.email);
        if(validateEmail){
            mouse.form.$emit('warningEmail');
            mouse.form.errorEmail = true;
            return false;
          //  let me = {form: this, email: this.emp.email, dateOfBirth: this.emp.dateOfBirth, identityIssuedDate: this.emp.identityIssuedDate}

        }
      
            //validate ngày sinh và ngày cấp CMND phải nhỏ hơn ngày hiện tại
        let dOB= common.checkDate(mouse.dateOfBirth, new Date());
        let identityIssuedDate = common.checkDate(mouse.identityIssuedDate, new Date());
        if(!dOB||!identityIssuedDate){
            if(!dOB ){
            mouse.form.$emit('warningMaxDateOfBirth'); 
            mouse.form.isDateOfBirth=true;  
            return false;                       
            } else{
                mouse.form.isDateOfBirth=false;            

            } 
            if( !identityIssuedDate){
                mouse.form.$emit('warningMaxDateIdentity'); 
                mouse.form.isIdentityIssuedDate=true;            
                return false;
            }  else{
                mouse.form.isIdentityIssuedDate=false;            
            } 
            if(mouse.form.isDateOfBirth===true|| mouse.form.isIdentityIssuedDate===true){
                return false;
            }
        }
       
           //validate ngày cấp CMND phải lớn hơn ngày sinh
        let minIdentityIssuedDate=common.checkDate(mouse.dateOfBirth,mouse.identityIssuedDate);
        if(!minIdentityIssuedDate){
            mouse.form.$emit('warningMinIdenDate');   
            mouse.form.identityDateLessDOB=true;
             return false;
        }
        
        return true;  
   }
   /* eslint-disable */ 
//    import { ref } from 'vue';
export default {
    components: { MPopupAsk,Datepicker},
    name: "employeeDetail",
    props: {
        closeDialog: Function,
        loadData: Function,
        employeeSelected: Function,
        formMode: Number,
        focus: Boolean,       
    },
    data() {
        return {           
            showForm: true,
            emp: {},          
            maxDate: common.maxDate(),
            chooseGender: null,
            dataCombobox: "",
            keyCombobox: null,
            emptyCode: false,
            emptyName: false,
            emptyDepartment: false,
            errorMaxCode:false,
            errorEmail: false,
            isPhone: false,
            isPhoneLand: false,
            isIdentity: false,
            isDateOfBirth: false,
            isIdentityIssuedDate: false,
            identityDateLessDOB:false,
            SaveAndAdd: eNum.saveFormMode.SaveAndAdd,
            Save: eNum.saveFormMode.Save,
            datetime: "",
            pickedUp: false,
            checkChange:false,
            showPopUpAsk: false,
            empOld:{},  
            apiGetDepartment:urlApi.methodCRUD.getDepartment,        
          
        };
    },
      created() {  
       //gán data truyền từ cha sang con
        this.emp =  this.employeeSelected; 
                                         
        //xử lý dữ liệu radio
        if (!common.checkEmptyData(this.emp.gender)) {
            this.chooseGender = this.emp.gender;
        }
        //xử lý dữ liệu combobox
        if (!common.checkEmptyData(this.emp.departmentName)) {
            this.dataCombobox = this.emp.departmentName;
            console.log(this.dataCombobox)
            this.emptyDepartment = false;
        }           
    },
    updated() {
        // initDragDialog();
       // drag();
        //update gender
        
        this.emp.gender = Number(this.chooseGender);      
    },
    mounted() {
        drag();     
        
        //nếu form được chọn là thêm mới hoặc nhân bản thì sẽ sinh mã nhân viên mới
        if (this.formMode === eNum.formMode.ADD || this.formMode === eNum.formMode.Duplicate) {
            this.newEmployeeCode();
        }
        this.$refs["employeeCode"].focus(); //focus vào item đầu tiên 
        this.formatDate(); //thay đổi dữ liệu ngày tháng theo format trước khi hiển thị  
      
        this.empOld={...this.employeeSelected};      
        //document.addEventListener("keydown",this.escClose);
    },
    unmounted(){
        
       
    },
    watch:{
        
        
        //focus vào textbox nếu gặp lỗi khi validate
        //CreatedBy: HMHieu(10/10/2022)
        focus(){                     
            // debugger
            //mã nhân viên trống hoặc quá dài
            if(this.emptyCode||this.errorMaxCode){
                this.$refs['employeeCode'].focus();
                this.$refs['employeeCode'].setAttribute('style','border:1px solid red');
            }else{
               
                this.$refs['employeeCode'].setAttribute('style','border:1px solid #bbbbbb;');
            }           

            if(this.emptyDepartment){
                if(this.emptyCode||this.errorMaxCode){
                this.$refs['employeeCode'].focus();
                this.$refs['employeeCode'].setAttribute('style','border:1px solid red');
                document.querySelectorAll(".combobox-container  .input")[0].setAttribute('style','border:1px solid red');

                 }
                
                else if(this.emptyName){
                this.$refs['employeeNameFocus'].focus();
                this.$refs['employeeNameFocus'].setAttribute('style','border:1px solid red');
                document.querySelectorAll(".combobox-container  .input")[0].setAttribute('style','border:1px solid red');


                }else if(this.emptyDepartment){
                   
                    document.querySelectorAll(".combobox-container .input")[0].focus();
                    document.querySelectorAll(".combobox-container  .input")[0].setAttribute('style','border:1px solid red');
                    // this.$refs['input'].focus();
                    // this.$refs['input'].setAttribute('style','border:1px solid red');
                }else{
                    document.querySelectorAll(".combobox-container  .input")[0].setAttribute('style','border:1px solid #bbbbbb');

                }
            }
            //email không đúng định dạng

            if(this.errorEmail){
                debugger
                if(this.emptyCode||this.errorMaxCode){
                this.$refs['employeeCode'].focus();
                this.$refs['employeeCode'].setAttribute('style','border:1px solid red');
                this.$refs['employeeNameFocus'].setAttribute('style','border:1px solid red');
                this.$refs['email'].setAttribute('style','border:1px solid red');

                 }
                
                else if(this.emptyName){
                this.$refs['employeeNameFocus'].focus();
                this.$refs['employeeNameFocus'].setAttribute('style','border:1px solid red');
                this.$refs['email'].setAttribute('style','border:1px solid red');

                }              
                else{
                    this.$refs['email'].focus();
                    this.$refs['email'].setAttribute('style','border:1px solid red');
                }
            }
            else {
                    this.$refs['email'].setAttribute('style','border:1px solid #bbbbbb');
                }
            //tên nhân viên trống

            if(this.emptyName){
                if(this.emptyCode||this.errorMaxCode){
                this.$refs['employeeCode'].focus();
                this.$refs['employeeCode'].setAttribute('style','border:1px solid red');
                }
                
                else if(this.emptyName){
                    this.$refs['employeeNameFocus'].focus();
                    this.$refs['employeeNameFocus'].setAttribute('style','border:1px solid red');
                    
                }
                
            }else {
                    this.$refs['employeeNameFocus'].setAttribute('style','border:1px solid #bbbbbb');
                }
            
           if(this.isIdentityIssuedDate){
            
            document.querySelectorAll(".identityIssuedDate .dp__input")[0].focus();
            // document.querySelectorAll(".identityIssuedDate .dp__input")[0].classList.add("border-red");
            document.querySelectorAll(".identityIssuedDate .dp__input")[0].setAttribute('style','border:1px solid red');

           }else{
            // document.querySelectorAll(".identityIssuedDate .dp__input")[0].classList.remove("border-red");
            document.querySelectorAll(".identityIssuedDate .dp__input")[0].setAttribute('style','border:1px solid #bbbbbb');                              

           }
            if(this.isDateOfBirth){
                document.querySelectorAll(".dateOfBirth .dp__input")[0].focus();
                document.querySelectorAll(".dateOfBirth .dp__input")[0].setAttribute('style','border:1px solid red');                              

            }else{
                document.querySelectorAll(".dateOfBirth .dp__input")[0].setAttribute('style','border:1px solid #bbbbbb');                              

                // document.querySelectorAll(".dateOfBirth .dp__input")[0].classList.remove("border-red");

            }  
            
            if(this.identityDateLessDOB){
                document.querySelectorAll(".identityIssuedDate .dp__input")[0].focus();            
                document.querySelectorAll(".identityIssuedDate .dp__input")[0].setAttribute('style','border:1px solid red');

            }else{
                
                document.querySelectorAll(".identityIssuedDate .dp__input")[0].setAttribute('style','border:1px solid #bbbbbb'); 
            }
           
            
        },
    },
    methods: {
        /**
         * 
         * @param {*} employeeChange 
         * Kiểm tra sự thay đổi của form khi ESC hoặc nút X
         * CreatedBy:HMHieu(07-10-22)
         */        
        askESC() {                 
            this.checkChange = false;
            this.showPopUpAsk = false;
            // if(this.formMode=== eNum.formMode.ADD){
            //     this.checkChange = true;
            // }else {
                for(let key in this.emp){
                    if(this.emp[key] !== this.empOld[key]){                   
                        this.checkChange = true;
                        break;
                    }
            //}
            }
           

            if(this.checkChange){
                this.showPopUpAsk = true;
            }else{
                this.closeDialog();             
            }
           
        },
        /**
         * Đóng PopUP hỏi
         * CreatedBY: HMHieu(07-10-22)
         */
        closeAsk() {
            this.showPopUpAsk = false;
        },
         /**
         * Đóng PopUP+Dialog hỏi
         * CreatedBY: HMHieu(07-10-22)
         */
        closeAskDialog() {
            this.showPopUpAsk = false;
            this.closeDialog();
        },
        /**
       * Đóng form dialog bằng nút hủy
       * Author: HMH(16/09/22)
      */
        closeEmpDetail() {
            this.closeDialog();
        },
       
        /**
         * format ngày tháng theo định dạng mong muốn
         * Author: HMH(19/09/22)
         */
        formatDate() {
            if (this.formMode === eNum.formMode.Edit) {
                this.emp.dateOfBirth = common.formatDate(this.emp.dateOfBirth);
                this.emp.identityIssuedDate = common.formatDate(this.emp.identityIssuedDate);
            }
        },
        /**
        * Check giá trị input rỗng khi focus vào input mã nhân viên
        * author: HMHieu(21/09/2022)
        *
        */
        validateEmptyCode() {
            if (common.checkEmptyData(this.emp.employeeCode)) {
                this.emptyCode = true;
               
            }
            else {
                this.emptyCode = false;
            }
        },
        /**
         *  kiểm tra xem nếu mã nhân viên vượt quá số ký tự quy định thì hiện thông báo
         * 
         */
        validateMaxCode(){
            if (!common.checkEmptyData(this.emp.employeeCode)) {
                if(this.emp.employeeCode.length>20){
                    this.errorMaxCode=true;
                }else{
                    this.errorMaxCode=false;
                }
            }
        },      
        /**
         * Check giá trị input có đúng định dạng number chưa
         * author: HMHieu(21/09/2022)
         *
         */
        validatePhoneNumber() {
            if (common.checkNumber(this.emp.mobile)) {
                this.isPhone = true;
            }
            else {
                this.isPhone = false;
            }
        },
        /**
        * Check giá trị input có đúng định dạng number chưa
        * author: HMHieu(21/09/2022)
        *
        */
        validatePhoneland() {
            if (common.checkNumber(this.emp.landlinePhone)) {
                this.isPhoneLand = true;
            }
            else {
                this.isPhoneLand = false;
            }
        },
        /**
        * Check giá trị input có đúng định dạng number chưa
        * author: HMHieu(21/09/2022)
        *
        */
        validateIdentity() {
            if (common.checkNumber(this.emp.identityNumber)) {
                this.isIdentity = true;
            }
            else {
                this.isIdentity = false;
            }
        },
        /**
         * Check giá trị input rỗng khi focus vào input tên nhân viên
         *  author: HMHieu(21/09/2022)
         *
         */
        validateEmptyName() {
            if (common.checkEmptyData(this.emp.employeeName)) {
                this.emptyName = true;
            }
            else {
                this.emptyName = false;
            }
        },
        /*
        * truyền giá trị cho combobox
        *  author: HMHieu(06/10/2022)
        */
        objectItemCombobox(item) {
            this.emp.departmentID = item.departmentID;
            this.emp.departmentName = item.departmentName;
            this.emp.DepartmentCode = item.departmentCode;
            this.empDepartmentName = this.emp.departmentName;
            if(this.emp.departmentName===null||this.emp.departmentName===undefined||this.emp.departmentName==='' ){
                this.emp.departmentName=item;
            }            
         
            // if (common.checkEmptyData(this.emp.departmentName) ||Object.keys(item).length === 0) {                          
            //     this.emptyDepartment = true;
            // }
            // else {
            //     this.emptyDepartment = false;
            // }
        
        },
        /**
         * validate giá trị phòng ban
         *  author: HMHieu(06/10/2022)
        */
        validateDepartment() {        
            if (common.checkEmptyData(this.emp.departmentName)||Object.keys(this.emp.departmentName).length === 0) {
                
                this.emptyDepartment = true;
            }
            else {
                this.emptyDepartment = false;
            }
        },      
        /**
        * Thêm mới hoặc sửa dữ liệu theo formMode
        * Author: HMH(19/09/22)
        */
       async btnSaveOnClick(modeForm) {
            try {
                
                // var me = this;
                var me = {form: this, email: this.emp.email, dateOfBirth: this.emp.dateOfBirth, identityIssuedDate: this.emp.identityIssuedDate}
                // validate dữ liệu:
                this.validateEmptyCode();                             
                this.validateMaxCode();       
                this.validateEmptyName();

                this.validateDepartment();  
                debugger 
                let checkEmpty = {
                    EmptyCode: this.emptyCode,
                    ErrorMaxCode:this.errorMaxCode,
                    EmptyName: this.emptyName,
                    EmptyDepartment: this.emptyDepartment,
                };
                                     
                 let valid=validate(me, checkEmpty);   
               
                //let valid=true;  
                if (valid) {
                    var method = "";
                    //cất
                    if (me.form.formMode === eNum.formMode.ADD) {//thêm mới dữ liệu
                        method = Resource.method.POST;
                       
                        try {
                            var res= await baseApi.fetchAPIBody(me.form.emp,urlApi.methodCRUD.insert,method)                                       
                            this.handleResponse(res,me.form,modeForm);// gọi hàm xử lí dữ liệu trả về       
                        } catch (error) {
                            this.$emit(Resource.emit.errorBackEnd,   eNum.errorBackEnd.Ser);                                                
                        }
                      
                                                 
                    }
                    // Cất sửa dữ liệu:
                    if (me.form.formMode === eNum.formMode.Edit) {//sửa dữ liệu
                        method =Resource.method.PUT;
                        try {
                            var res= await baseApi.fetchAPIBody(me.form.emp,urlApi.apiDeleteOrPut(me.form.emp.employeeID),method)                                                      
                            this.handleResponse(res,me.form,modeForm);// gọi hàm xử lí dữ liệu trả về                                                
                        } catch (error) {
                            this.$emit(Resource.emit.errorBackEnd,   eNum.errorBackEnd.Ser);                                                
                        }    
                    }
                    // Nhân bản dữ liệu
                    if (me.form.formMode === eNum.formMode.Duplicate) {
                        method = Resource.method.POST;
                        try {
                            var res= await baseApi.fetchAPIBody(me.form.emp,urlApi.methodCRUD.insert,method)                 
                            this.handleResponse(res,me.form,modeForm);// gọi hàm xử lí dữ liệu trả về                                          
                        } catch (error) {
                            this.$emit(Resource.emit.errorBackEnd,   eNum.errorBackEnd.Ser);                                                
                        }    
                                     
                    }
                }
            } catch (error) {
                //thông báo lỗi cho người dùng khi gặp ex
                this.$emit(Resource.emit.errorBackEnd, eNum.errorFE.UserActionError);           
                console.log(error);
            }
        },

        /**
         * 
         * handle trạng thái trả về từ BE
         * @param {*} res // thông tin trả về từ BE
         * @param {*} me //con trỏ
         * @param {*} mode // formMode
         * CreatedBy HMHieu (20-10-2022)
         */
        handleResponse(res,me,mode){                                         
            let ok = res.success;
       // debugger
            if(ok){//Thành công
                me.$emit(Resource.emit.openToast, Resource.ToastMessage.success);
                me.$emit(Resource.emit.loadData);
                if (mode === eNum.saveFormMode.Save) {//nếu là cất
                    me.$emit(Resource.emit.closeDialogAdd);
                }
                if (mode === eNum.saveFormMode.SaveAndAdd) {//nếu là cất và thêm mới
                    me.$emit(Resource.emit.loadData);
                    me.$emit(Resource.emit.reloadForm);
                    me.newEmployeeCode();  //sinh code mới   
                    me.$refs['employeeCode'].focus();    //focus vào employeeCode
                }
            }else{ // thất bại
                debugger;
                let errorCode = res.data.errorCode;  //Mã lỗi trả về được quy định ở BE
                let content=res.data.userMsg; // Message lỗi trả về tương ứng với mã lỗi từ BE
                if (
                    errorCode === eNum.errorBackEnd.InsertFailCode||
                    errorCode === eNum.errorBackEnd.exception) {
                    me.$emit(Resource.emit.errorBackEnd,  errorCode,content);//gọi hàm handle errorCode
                }
                else{
                    //thông báo lỗi cho người dùng 
                    me.$emit(Resource.emit.errorBackEnd,   eNum.errorBackEnd.Ser);                    
                }
                //hiển thị toast message 
                   // me.$emit("openToast", Resource.ToastMessage.error);
            }    
        },
        /**
      *
      * @param {*} event
      * Thực hiện chức năng đóng form bằng ESC
      * CreatedBy HMHieu
      */
        escClose(event) {
            let me = this;
            try {
                let key = event.keyCode;
                switch (key) {
                    case eNum.KeyCode.ESC:
                        me.askESC();                       
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        
        /********************
        * đóng form bằng keycode ESC
        * HMHieu 17-11-2022
        * 
        // */       
        // escClose(event) {       
        //     debugger                                 
        //     let key = event.keyCode;
        //     if(key===eNum.KeyCode.ESC){
        //         this.askESC();
        //     }
                    
        // },
        /**
        *
        * @param {*} event
        * đóng form bằng keyCOde
        * CreatedBy HMHieu
        */
        keyCancel(event) {
            let me = this;
            try {
                let key = event.keyCode;
                switch (key) {
                    case eNum.KeyCode.Enter:
                        me.$emit(Resource.emit.closeDialogAdd);
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        /**
         *
         * @param {*} event
         * Lưu bằng keyCOde
         * CreatedBy HMHieu
         */
        saveBtn(event) {
            // let me=this;
            try {
                if (event.keyCode === eNum.KeyCode.S && event.ctrlKey) {
                    event.preventDefault();
                    this.btnSaveOnClick(eNum.saveFormMode.Save);
                }
                if (event.keyCode === eNum.KeyCode.S && event.ctrlKey && event.shiftKey) {
                    event.preventDefault();
                    this.btnSaveOnClick(eNum.saveFormMode.SaveAndAdd);
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        /**
        *
        * @param {*} event
        * Thực hiện chức năng Khi focus vào button và nhấn Enter
        * CreatedBy HMHieu
        */
        keySave(event, mode) {
            // let me=this;
           
            let key = event.keyCode;
            switch (key) {
                case eNum.KeyCode.Enter:
                    if (mode === eNum.saveFormMode.Save) {
                        this.btnSaveOnClick(eNum.saveFormMode.Save);
                    }
                    if (mode === eNum.saveFormMode.SaveAndAdd) {
                        this.btnSaveOnClick(eNum.saveFormMode.SaveAndAdd);
                    }
            }
          
        },
        /**
       * Hàm lấy mã nhân viên mới nhất
       * author: HMHieu(07/10/2022)
       */
       async newEmployeeCode() {
            let me=this;
            let method=Resource.method.GET;
            try {
                var res= await baseApi.fetchAPIDefault(urlApi.methodCRUD.getNewCode,method);                  
                this.handleResponseGetNewCode(res,me);               
            }
            catch (error) {
                console.log(error);
                me.$emit(Resource.emit.errorBackEnd,   eNum.errorBackEnd.Ser); 
            }
        },

        /**
         * xử lý dữ liệu trả về khi getData
         * @param {*} res response data trả về từ BE
         * @param {*} me con trỏ
         * 
         */
         handleResponseGetNewCode(res,me){                                        
            let errorCode = res.errorCode;                     
            let ok = res.success;          
            if(ok){//thành công
                me.emp.employeeCode = res.data;                
                me.emptyCode = false;                           
            }else{//bắt lỗi trả về từ BE
                if (
                    errorCode === eNum.errorBackEnd.InsertFailCode||
                    errorCode === eNum.errorBackEnd.exception) {
                    me.$emit(Resource.emit.errorBackEnd,  errorCode);
                }
            }                 
        },
    },
   
}
</script>