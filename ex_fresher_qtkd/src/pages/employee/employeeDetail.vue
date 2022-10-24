<template>
     <!-- Detail Employee  -->
     <div class="dialog__wrap"   > 
       
          <!-- <div class="dialog__infoEmp" @keyup="escClose($event)" @keydown="saveBtn($event)"
          @mousemove="draging($event)"  
          @mouseup="drop"  
          @mousedown="drag"  
          :ref="'headerDialog'" > -->
          <div class="dialog__infoEmp" @keyup="escClose($event)" @keydown="saveBtn($event)">
          
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
                                    @focus = 'validateEmptyCode'
                                    @blur = 'validateEmptyCode'
                                    :class="{'border-red' : emptyCode}"
                                    v-model="emp.employeeCode"
                                    :ref="'employeeCode'" >
                                    <div class="empty__input tooltiptext tooltipError"  v-show="emptyCode" >Mã không được để trống</div>
                                </div>
                              
                               
                            </div>
                            <div class="info__item info__name">
                                <div class="lable-input">
                                    Tên
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input tooltip">
                                    <input type="text" class="input"   maxlength="100"
                                    @keyup = 'validateEmptyName'
                                    @focus = 'validateEmptyName'
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
                            
                                 <MCombobox :url="'http://localhost:5063/api/v1/Departments'" 
                                 :code="'departmentCode'" 
                                :text="'departmentName'" 
                                @objectItemCombobox='objectItemCombobox'
                                :valueRender='dataCombobox'
                                @blur ='validateDepartment'
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
                                <div class="content__input">                                  
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
                                    :ref="'dateOfBirth'"
                                       v-model="emp.dateOfBirth"/>                 
                                    <!-- <input class="input input__date"   type="date" v-model="emp.dateOfBirth" :max="maxDate" > -->
                                </div>
                               
                            </div>
                            <div class="info__item item__radio">
                                <div class="lable-input label-gender">
                                  Giới tính
                                    
                                </div>
                                <div class="content__radio">
                                    
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio " checked   id="radio__boy"  value="0" name="gender"                                           
                                         v-model="chooseGender"                                       
                                         >
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
                                <div class="content__input">
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
                                 @input="validatePhoneNumber" 
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
                                 @input="validatePhoneland"
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
                    <button class="btn btn__saveAdd tooltip"  @click="btnSaveOnClick(this.SaveAndAdd)"  @keydown="keySave($event,this.SaveAndAdd)" >
                        Cất và thêm
                        <span class="tooltiptext tooltipIcon tooltip__large tooltip__right">Cất và thêm (Ctrl + Shift + S)</span>
                    </button>
                </div>
            </div>
        
        </div> 
      
    </div>
       <!-- End detail Employee -->
       <!-- <MPopupAsk />  -->
        <MPopupAsk v-show="showPopUpAsk" @btnSaveOnClick="btnSaveOnClick"  @closeAsk='closeAsk' @closeAskDialog='closeAskDialog'></MPopupAsk>
</template>
<script>
    
    // import MCombobox from '@/components/base/MCombobox.vue';
    import eNum from '@/js/common/eNum';
    import common from '@/js/common/common';
    import Resource from '@/js/common/resource';
    // import { vn } from 'date-fns/locale';
    import MPopupAsk from '../../components/base/MPopupAsk.vue';
    import Datepicker from '@vuepic/vue-datepicker';
    import '@vuepic/vue-datepicker/dist/main.css';
    // import MPopupAsk from '@/components/base/MPopupAsk.vue';


    /**
     * URL api 
     */
    const loadDataURL =process.env.VUE_APP_URL;
    // var loadDataURL="http://localhost:5287/api/v1/";

     /**
     * Hàm validate
     * author: HMHieu(14/9/2022)
     * @param {*} form: con trỏ vue
     * @param {object} objectEmpty {emptyCode, emptyName, emptyDepartment} 

     * 
     */
     function validate(mouse, objectEmpty){
        
        for(let key in objectEmpty){
            if(objectEmpty[key]){                             
                mouse.form.$emit('errorEmpty', key); 
                console.log(objectEmpty[key]);        
                return false;                
            }
        }  
        let validateEmail = common.checkEmail(mouse.email);
        if(validateEmail){
            mouse.form.$emit('waringEmail');
            mouse.form.errorEmail = true;
            return false;
          //  let me = {form: this, email: this.emp.email, dateOfBirth: this.emp.dateOfBirth, identityIssuedDate: this.emp.identityIssuedDate}

        }
        // else{
        //     mouse.form.errorEmail = false;
        // }

        let dOB= common.checkDate(mouse.dateOfBirth, new Date());
        let identityIssuedDate = common.checkDate(mouse.identityIssuedDate, new Date());
        if(!dOB || !identityIssuedDate){
            mouse.form.$emit('waringMaxDate');
            // if(!dOB){
            //     mouse.form.isDateOfBirth = true;
            // }
            // else if(dOB){
            //     mouse.form.isDateOfBirth = false;
            // }
            // else if(!identityIssuedDate){
            //     if(!dOB){
            //     mouse.form.isDateOfBirth = true;
            //     }else{
            //         mouse.form.isIdentityIssuedDate = true;
            //     }               
            // }           
            // else{
            //     mouse.form.isIdentityIssuedDate = false;
            // }
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
            errorEmail: false,
            isPhone: false,
            isPhoneLand: false,
            isIdentity: false,
            isDateOfBirth: false,
            isIdentityIssuedDate: false,
            SaveAndAdd: eNum.saveFormMode.SaveAndAdd,
            Save: eNum.saveFormMode.Save,
            datetime: "",
            pickedUp: false,
            checkChange:false,
            showPopUpAsk: false,
            empOld:{},          
          
        };
    },
    created() {   
           
            //gán data truyền từ cha sang con
            this.emp = this.employeeSelected;
          
        //xử lý dữ liệu radio
        if (!common.checkEmptyData(this.emp.gender)) {
            this.chooseGender = this.emp.gender;
        }
        //xử lý dữ liệu combobox
        if (!common.checkEmptyData(this.emp.departmentName)) {
            this.dataCombobox = this.emp.departmentName;
            this.emptyDepartment = false;
        }           
    },
    updated() {
        //update gender
        this.emp.gender = Number(this.chooseGender);      
    },
    mounted() {
        if (this.formMode === eNum.formMode.ADD || this.formMode === eNum.formMode.Duplicate) {
            this.newEmployeeCode();
        }
        this.$refs["employeeCode"].focus(); //focus vào item đầu tiên 
        this.formatDate(); //thay đổi dữ liệu ngày tháng theo format trước khi hiển thị       
        this.empOld={...this.employeeSelected};      
    },
    watch:{
        focus(){           
          debugger;

            if(this.emptyCode){
                this.$refs['employeeCode'].focus();
            }
            if(this.errorEmail){
                if(this.emptyCode){
                this.$refs['employeeCode'].focus();
                 }
                else if(this.emptyName){
                this.$refs['employeeNameFocus'].focus();
                }
                else{
                    this.$refs['email'].focus();
                } 
            }
            if(this.emptyName){
                if(this.emptyCode){
                this.$refs['employeeCode'].focus();
                }
                else{this.$refs['employeeNameFocus'].focus();}
            }
            // if(this.isDateOfBirth||this.isIdentityIssuedDate){
            //     if(this.emptyCode){
            //     this.$refs['employeeCode'].focus();
            //      }
            //     else if(this.emptyName){
            //     this.$refs['employeeNameFocus'].focus();
            //     }
            //     else if(this.isDateOfBirth){
            //         this.$refs['dateOfBirth'].focus();
            //     }
            //     else if(this.isIdentityIssuedDate){
            //         this.$refs['identityIssuedDate'].focus();
            //     }
            //     this.$refs['dateOfBirth'].focus();
            // }
            // if(this.isIdentityIssuedDate && this.isDateOfBirth ){
            //     if(this.emptyCode){
            //     this.$refs['employeeCode'].focus();
            //      }
            //     else if(this.emptyName){
            //     this.$refs['employeeNameFocus'].focus();
            //     }
            //     this.$refs['dateOfBirth'].focus();
            // }
           
            
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
            for(let key in this.emp){
                if(this.emp[key] !== this.empOld[key]){                   
                    this.checkChange = true;
                    break;
                }
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
         * Check giá trị email có đúng định dạng chưa khi nhập dữ liệu vào input
         * author: HMHieu(21/09/2022)
         *
         */
        validateEmail() {
            if (common.checkEmail(this.emp.email)) {
                this.errorEmail = true;
                this.$emit('warningEmail');
            }
            else {
                this.errorEmail = false;
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

          
            if (common.checkEmptyData(this.emp.departmentName)) {                          
                this.emptyDepartment = true;
            }
            else {
                this.emptyDepartment = false;
            }
        
        },
        /**
         * validate giá trị phòng ban
         *  author: HMHieu(06/10/2022)
        */
        validateDepartment() {
            if (common.checkEmptyData(this.emp.departmentName)) {
                
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
        btnSaveOnClick(modeForm) {
            // var me = this;
             var me = {form: this, email: this.emp.email, dateOfBirth: this.emp.dateOfBirth, identityIssuedDate: this.emp.identityIssuedDate}
            // validate dữ liệu:
            this.validateEmptyName();
            this.validateEmptyCode();
            this.validateDepartment();          
            let checkEmpty = {
                EmptyCode: this.emptyCode,
                EmptyName: this.emptyName,
                EmptyDepartment: this.emptyDepartment
            };
         
          
            let valid = validate(me, checkEmpty);
           // let valid =true;
            
            if (valid) {
                //cất
                if (me.form.formMode === eNum.formMode.ADD) {
                    var method = "POST";
                    fetch(loadDataURL + "Employees", {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.form.emp),
                    })                                             
                        .then((res) => res.json())
                        .then((res) => {
                            this.handleResponse(res,me.form,modeForm);
                      
                    })
                        .catch((res) => {
                            me.form.$emit("errorBackEnd", eNum.errorBackEnd.Ser);                              
                            console.log(res);
                            me.form.$emit("openToast", Resource.ToastMessage.error);
                    });
                }
                // Cất sửa dữ liệu:
                if (me.form.formMode === eNum.formMode.Edit) {
                    method = "PUT";
                    let URL = loadDataURL + `Employees/${me.form.emp.employeeID}`;
                    console.log(URL);
                    fetch(URL, {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.form.emp),
                    })
                        .then((res) => res.json())
                        .then((res) => {
                            this.handleResponse(res,me.form,modeForm);
                                            
                    })
                        .catch((res) => {                      
                            me.form.$emit("errorBackEnd", eNum.errorBackEnd.Ser);      
                            console.log(res);
                            me.form.$emit("openToast", Resource.ToastMessage.error);
                    });
                }
                if (me.form.formMode === eNum.formMode.Duplicate) {
                    method = "POST";
                    fetch(loadDataURL + "Employees", {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.form.emp),
                    })
                        .then((res) => res.json())
                        .then((res) => {                       
                             this.handleResponse(res,me.form,modeForm);                            
                    })
                        .catch((res) => {                                                    
                            me.form.$emit("errorBackEnd", eNum.errorBackEnd.Ser);    
                            console.log(res);
                            me.form.$emit("openToast", Resource.ToastMessage.error);                      
                    });
                }
            }
        },
        /**
         * 
         * @param {*} res // thông tin trả về từ BE
         * @param {*} me //con trỏ
         * @param {*} mode // formMode
         * handle trạng thái trả về từ BE
         * CreatedBy HMHieu (20-10-2022)
         */
        handleResponse(res,me,mode){          
            let errorCode = res.data.errorCode;  
            let content=res.data.userMsg;                   
            let ok = res.success;
            if(ok){
                this.$emit("openToast", Resource.ToastMessage.success);
                me.$emit("loadData");
                if (mode === eNum.saveFormMode.Save) {
                    me.$emit("closeDialog");
                }
                if (mode === eNum.saveFormMode.SaveAndAdd) {
                    me.$emit("loadData");
                    me.$emit('reLoadForm');
                    me.newEmployeeCode();     
                    this.$refs['employeeCode'].focus();    
                }
            }else{                      
                if (
                    errorCode === eNum.errorBackEnd.InsertFailCode||
                    errorCode === eNum.errorBackEnd.exception) {
                    me.$emit("errorBackEnd",  errorCode,content);
                }
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
                       // me.$emit("closeDialog");
                }
            }
            catch (error) {
                console.log(error);
            }
        },
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
                        me.$emit("closeDialog");
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
            try {
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
            }
            catch (error) {
                console.log(error);
            }
        },
        /**
       * Hàm lấy mã nhân viên mới nhất
       * author: HMHieu(07/10/2022)
       */
        newEmployeeCode() {
            let me=this;
            try {
                fetch(loadDataURL + "Employees/new-code-employee", { method: "GET" })
                    .then(res => res.json())
                    .then(res => {

                        let errorCode = res.errorCode;                     
                        let ok = res.success;
                        console.log(ok);
                        if(ok){
                            me.emp.employeeCode = res.data;
                           
                            me.emptyCode = false;                           
                        }else{                           
                            if (
                                errorCode === eNum.errorBackEnd.InsertFailCode||
                                errorCode === eNum.errorBackEnd.exception) {
                                me.$emit("errorBackEnd",  errorCode);
                            }
                        }                  
                   
                })
                    .catch(error => {
                    me.$emit("errorBackEnd", eNum.errorBackEnd.Ser); 
                    console.log(error);
                    return null;
                });
            }
            catch (error) {
                console.log(error);
            }
        },
    },
   
}
</script>