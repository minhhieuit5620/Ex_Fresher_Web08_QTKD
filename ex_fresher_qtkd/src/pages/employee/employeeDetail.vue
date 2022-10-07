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
                        <div class="dialog__close icon icon__close"  @click="askESC(this.emp)" tabIndex="23" ></div>
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
                                    <input type="text" class="input"  tabIndex="1" maxlength="20"
                                    @keyup = 'validateEmptyCode'
                                    @focus = 'validateEmptyCode'
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
                                    <input type="text" class="input"  tabIndex="2" maxlength="100"
                                    @keyup = 'validateEmptyName'
                                    @focus = 'validateEmptyName'
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
                                 @forcus="validateDepartment"    
                                :key = 'keyCombobox' 
                                :class="{'border-red': emptyDepartment}"
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
                                <input class="input" type="text"   tabIndex="4" maxlength="255" v-model="emp.positionName">
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
                                    <datePicker 
                                    :tabIndex=5 class="datepickerCTM" 
                                    placeholder="DD/MM/YYYY"
                                    :enterSubmit=true
                                    :tabSubmit=true
                                  
                                    :maxDate="new Date()"
                                     format="dd/MM/yyyy" 
                                    textInput 
                                    :dayNames="['T2', 'T3', 'T4', 'T5', 'T6', 'T7','CN']"
                                   
                                    
                                    autoApply 
                                   
                                    utc 
                                    locale="vn"
                                       v-model="emp.dateOfBirth">
                                    
                                     
                                     
                                         
                                        
                                    </datePicker>
                                       
                                    <!-- <input class="input input__date"   type="date" v-model="emp.dateOfBirth" :max="maxDate" > -->
                                </div>
                               
                            </div>
                            <div class="info__item item__radio">
                                <div class="lable-input label-gender">
                                  Giới tính
                                    
                                </div>
                                <div class="content__radio">
                                    
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio " checked  tabIndex="6" id="radio__boy"  value="0" name="gender"                                           
                                         v-model="chooseGender"                                       
                                         >
                                        <label for="radio__boy">Nam</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio "  tabIndex="7" id="radio__girl" value="1" name="gender"                                         
                                        v-model="chooseGender"                                      
                                        >
                                        <label for="radio__girl">Nữ</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio "  tabIndex="8"  id="radio__other" value="2" name="gender"                                             
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
                                    <input class="input"  tabIndex="9"
                                     maxlength="25" type="text" 
                                     v-model="emp.identityNumber"
                                     @input="validateIdentity"
                                     :class="{'border-red': isIdentity}"
                                     
                                      >
                                      <div class="empty__input tooltiptext tooltipError"  v-show="isIdentity">Số CMND không đúng</div>   
                                </div>  
                                                         
                            </div>
                            <div class="info__item ">
                                <div class="lable-input">
                                  Ngày cấp                                    
                                </div>
                                <div class="content__input">
                                    <!-- <datePicker v-model="emp.identityIssuedDate" :format="format" /> -->
                                    <datePicker tabIndex="10" class="datepickerCTM" 
                                    placeholder="DD/MM/YYYY"
                                    :maxDate="new Date()"                                    
                                     format="dd/MM/yyyy" 
                                    textInput 
                                    :dayNames="['T2', 'T3', 'T4', 'T5', 'T6', 'T7','CN']"
                                    autoApply 
                                    utc 


                                    :enterSubmit=true
                                    :tabSubmit=true
                                   

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
                                <input class="input" type="text"  tabIndex="11"  maxlength="255" v-model="emp.identityIssuedPlace" >
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
                                <input type="text" class="input"  tabIndex="12" maxlength="255" v-model="emp.address">
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
                                 @input="validatePhoneNumber" tabIndex="13"
                                 v-model="emp.mobile"
                                 :class="{'border-red': isPhone}"
                                 >
                                 <div class="empty__input tooltiptext tooltipError"  v-show="isPhone">Số điện thoại không đúng</div>  
                             </div>
                                 
                        </div>
                        <div class=" info__item item__other">
                            <div class="lable-input tooltip tooltip__orther">
                                ĐT cố định      
                                <span class="tooltiptext tooltipAronym">Điện thoại cố định</span>                             
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  tabIndex="14"  maxlength="50" 
                                 @input="validatePhoneland"
                                 v-model="emp.landlinePhone"
                                 :class="{'border-red': isPhoneLand}"
                                 >
                                 <div class="empty__input tooltiptext tooltipError"  v-show="isPhoneLand">Số điện thoại không đúng</div>  
                             </div>
                             
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Email                                
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  
                                 tabIndex="15"
                                 :class="{'border-red': errorEmail}"  
                                 maxlength="100" 
                                 @input="validateEmail"
                                 v-model="emp.email">
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
                                 <input type="text" class="input" tabIndex="16" maxlength="25" v-model="emp.bankAccount">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tên ngân hàng                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" tabIndex="17"  maxlength="255" v-model="emp.bankName">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Chi nhánh                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input" tabIndex="18" maxlength="255" v-model="emp.bankBranch">
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
                    <div class="btn btn__cancel tooltip" @click="closeEmpDetail" tabIndex="21" @keydown="keyCancel($event)">
                        Hủy
                        <span class="tooltiptext tooltipIcon">Huỷ</span>
                    </div>
                   
                </div>
                <div class="dialog__right">
                    <div class="btn btn__save tooltip" tabIndex="19" @click="btnSaveOnClick(this.Save)"  @keydown="keySave($event,this.Save)">
                        Cất
                        <span class="tooltiptext tooltipIcon">Cất (Ctrl + S)</span>
                    </div>
                    <div class="btn btn__saveAdd tooltip" tabIndex="20" @click="btnSaveOnClick(this.SaveAndAdd)" @keydown="keySave($event,this.SaveAndAdd)" >
                        Cất và thêm
                        <span class="tooltiptext tooltipIcon tooltip__large tooltip__right">Cất và thêm (Ctrl + Shift + S)</span>
                    </div>
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
    import { vn } from 'date-fns/locale';
    import MPopupAsk from '../../components/base/MPopupAsk.vue';
  
    // import MPopupAsk from '@/components/base/MPopupAsk.vue';


    /**
     * URL api 
     */
    var loadDataURL =process.env.VUE_APP_URL;
    // var loadDataURL="http://localhost:5287/api/v1/";

     /**
     * Hàm validate
     * author: HMHieu(14/9/2022)
     * @param {*} form: con trỏ vue
     * @param {object} objectEmpty {emptyCode, emptyName, emptyDepartment} 

     * 
     */
     function validate(form, objectEmpty){
        for(let key in objectEmpty){
            if(objectEmpty[key]){                             
                form.$emit('errorEmpty', key); 
                console.log(objectEmpty[key]);        
                return false;
                
            }
        }       
        return true;  
   }
//    import { ref } from 'vue';
export default {
    components: { MPopupAsk },
    name: "employeeDetail",
    props: {
        closeDialog: Function,
        loadData: Function,
        employeeSelected: Function,
        formMode: Number,
        /**
         * `date-fns`-type formatting for a month view heading
         */
        monthHeadingFormat: {
            type: String,
            required: false,
            default: "LLLL yyyy",
        },
    },
    data() {
        return {
            vn: vn,
            showForm: true,
            emp: {},
            test: {},
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
            SaveAndAdd: eNum.saveFormMode.SaveAndAdd,
            Save: eNum.saveFormMode.Save,
            datetime: "",
            pickedUp: false,
            showPopUpAsk: false,
        };
    },
    created() {
        console.log(this.emp);    
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
    },
    methods: {
        /**
         * 
         * @param {*} employeeChange 
         * Kiểm tra sự thay đổi của form khi ESC hoặc nút X
         * CreatedBy:HMHieu(07-10-22)
         */        
        askESC(employeeChange) {
            this.showPopUpAsk = true;
            if (this.formMode === eNum.formMode.ADD) {
                if (employeeChange != {}) {
                    this.showPopUpAsk = true;
                }
                else {
                    this.closeDialog();
                    this.showPopUpAsk = false;
                }
            }
            if (this.formMode === eNum.formMode.Edit) {
                if (employeeChange != this.emp) {
                    this.showPopUpAsk = true;
                }
                else {
                    this.closeDialog();
                    this.showPopUpAsk = false;
                }
            }
            console.log(employeeChange);
            console.log("defaul ", this.empSelected);
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
            var me = this;
            // validate dữ liệu:
            let checkEmpty = {
                EmptyCode: this.emptyCode,
                EmptyName: this.emptyName,
                EmptyDepartment: this.emptyDepartment
            };
            this.validateEmptyName();
            this.validateEmptyCode();
            this.validateDepartment();
            let valid = validate(me, checkEmpty);
            if (valid) {
                //cất
                if (me.formMode === eNum.formMode.ADD) {
                    var method = "POST";
                    fetch(loadDataURL + "Employees", {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.emp),
                    })
                        .then((res) => res.json())
                        .then((res) => {
                        let status = res.status;
                        let errorCode = res.errorCode;
                        console.log(status);
                        if (errorCode === eNum.errorBackEnd.DuplicateCode) {
                            me.$emit("errorDuplicate", me.emp.employeeCode);
                        }
                        if (status === eNum.errorBackEnd.Ser || status === eNum.errorBackEnd.User || status === eNum.errorBackEnd.NotFound || errorCode === eNum.errorBackEnd.InsertFailCode) {
                            me.$emit("errorBackEnd", status || errorCode);
                        }
                        else {
                            this.$emit("openToast", Resource.ToastMessage.success);
                            me.$emit("loadData");
                            if (modeForm === eNum.saveFormMode.Save) {
                                me.$emit("closeDialog");
                            }
                            if (modeForm === eNum.saveFormMode.SaveAndAdd) {
                                me.emp = {};
                            }
                        }
                    })
                        .catch((res) => {
                        console.log(res);
                        this.$emit("openToast", Resource.ToastMessage.error);
                    });
                }
                // Cất sửa dữ liệu:
                if (me.formMode === eNum.formMode.Edit) {
                    method = "PUT";
                    let URL = loadDataURL + `Employees/${me.emp.employeeID}`;
                    console.log(URL);
                    fetch(URL, {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.emp),
                    })
                        .then((res) => res.json())
                        .then((res) => {
                        let status = res.status;
                        console.log(status);
                        let errorCode = res.errorCode;
                        if (errorCode === eNum.errorBackEnd.DuplicateCode) {
                            me.$emit("errorDuplicate", me.emp.employeeCode);
                        }
                        if (status === eNum.errorBackEnd.Ser || status === eNum.errorBackEnd.User || status === eNum.errorBackEnd.NotFound || errorCode === eNum.errorBackEnd.InsertFailCode) {
                            me.$emit("errorBackEnd", status || errorCode);
                        }
                        else {
                            this.$emit("openToast", Resource.ToastMessage.success);
                            me.$emit("loadData");
                            if (modeForm === eNum.saveFormMode.Save) {
                                me.$emit("closeDialog");
                            }
                            if (modeForm === eNum.saveFormMode.SaveAndAdd) {
                                me.emp = {};
                            }
                        }
                        //     else if(status===eNum.errorBackEnd.Success){
                        //         this.$emit('openToast', Resource.ToastMessage.success);
                        //         me.$emit("loadData");   
                        //         if(modeForm===eNum.saveFormMode.Save){
                        //             me.$emit("closeDialog");  
                        //         } 
                        //         if(modeForm===eNum.saveFormMode.SaveAndAdd){
                        //             me.emp={}; 
                        //         }         
                        //     }              
                        // //    else if(status===eNum.errorBackEnd.Ser||eNum.errorBackEnd.User||eNum.errorBackEnd.NotFound){
                        // //         me.$emit('errorBackEnd',status); 
                        // //         //this.$emit('openToast', Resource.ToastMessage.error);                          
                        // //     }
                        //     else{ 
                        //         me.$emit('errorBackEnd',status);       
                        //     }
                    })
                        .catch((res) => {
                        console.log(res);
                        this.$emit("openToast", Resource.ToastMessage.error);
                    });
                }
                if (me.formMode === eNum.formMode.Duplicate) {
                    method = "POST";
                    fetch(loadDataURL + "Employees", {
                        method: method,
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(me.emp),
                    })
                        .then((res) => res.json())
                        .then((res) => {
                        let status = res.status;
                        let errorCode = res.errorCode;
                        console.log(status);
                        if (errorCode === eNum.errorBackEnd.DuplicateCode) {
                            me.$emit("errorDuplicate", me.emp.employeeCode);
                        }
                        else if (status === eNum.errorBackEnd.Ser || eNum.errorBackEnd.User || eNum.errorBackEnd.NotFound) {
                            me.$emit("errorBackEnd", status);
                            //this.$emit('openToast', Resource.ToastMessage.error);                          
                        }
                        else {
                            this.$emit("openToast", Resource.ToastMessage.success);
                            me.$emit("loadData");
                            if (modeForm === eNum.saveFormMode.Save) {
                                me.$emit("closeDialog");
                            }
                            if (modeForm === eNum.saveFormMode.SaveAndAdd) {
                                me.emp = {};
                                //  me.emp.employeeCode= this.newEmployeeCode();
                            }
                        }
                    })
                        .catch((res) => {
                        console.log(res);
                        this.$emit("openToast", Resource.ToastMessage.error);
                    });
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
                if (event.keyCode == eNum.KeyCode.S && event.ctrlKey) {
                    event.preventDefault();
                    this.btnSaveOnClick(eNum.saveFormMode.Save);
                }
                if (event.keyCode == eNum.KeyCode.S && event.ctrlKey && event.shiftKey) {
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
            try {
                fetch(loadDataURL + "Employees/new-code-employee", { method: "GET" })
                    .then(res => res.text())
                    .then(data => {
                    this.emp.employeeCode = data;
                    this.emptyCode = false;
                })
                    .catch(error => {
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