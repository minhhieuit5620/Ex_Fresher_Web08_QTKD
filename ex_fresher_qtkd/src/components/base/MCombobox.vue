<template>
  <div class="combobox-container" @keydown="directItemDownUp($event)" @keyup="openDataCombobox($event)">
    <div class="input-container"  >
      <input type="text" class=" input" @input="search" v-model="dataSelected"  @focus="comboboxFocus"  :class="{'border-red': emptyCombobox}"  :ref="'input'">
      <div id="icon-combobox" class="icon-combobox" @click="showDataList=!showDataList"  >
        <div class="icon-drop-combobox" @click="loadDepartment"></div>
      </div>
    </div>
    <div class="combobox-value" v-if="showDataList" v-clickoutside="hideListData" :ref="'dataList'">
      <div class="combobox-header">
        <div class="code__combobox">Mã đơn vị</div>
        <div class="value__combobox">Tên đợn vị</div>
      </div>

      <div class="combobox-item" v-for="(item, index) in items" :key="index" @click="selectedDataCombobox(item, index)"
        :class="{'combobox-item-active': selected == index}" v-show="isSearch[index]" :ref="'focus_' +index"
       >

        <div class="code__combobox--value">{{item[this.code]}}</div>
        <div class="value__combobox--value"> {{item[this.text]}}</div>


        <div class="icon-tick" v-show="selected==index"></div>
      </div>
    </div>
  </div>
</template>

<script>
import common from '@/js/common/common';
/**
* Hàm xử lý dữ liệu tiếng việt
* author: HMHieu(11/9/2022)
* @param {String} xử lý dữ liệu trong tìm kiếm
*/
function processData(Text) {
  Text = Text.trim().toLowerCase();
  //Xoa khoảng trắng
  Text = Text.replace(/\s\s+/g, ' ');
  //Xóa dấu tiếng việt trong chuỗi
  Text = Text.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
  Text = Text.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
  Text = Text.replace(/ì|í|ị|ỉ|ĩ/g, "i");
  Text = Text.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
  Text = Text.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
  Text = Text.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
  Text = Text.replace(/đ/g, "d");
  Text = Text.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "");

  return Text;
}
/* eslint-disable */
/**
 * Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
 * HMHieu (09/10/2022)
 */
 const clickoutside = {
  mounted(el, binding, vnode, prevVnode) {
    el.clickOutsideEvent = (event) => {
      // Nếu element hiện tại không phải là element đang click vào
      // Hoặc element đang click vào không phải là button trong combobox hiện tại thì ẩn đi.
      if (
        !(
          (
            el === event.target || // click phạm vi của combobox__data
            el.contains(event.target) || //click vào element con của combobox__data
            el.previousElementSibling.contains(event.target)
          ) //click vào element button trước combobox data (nhấn vào button thì hiển thị)
        )
      ) {
        // Thực hiện sự kiện tùy chỉnh:
        binding.value(event, el);
        // vnode.context[binding.expression](event); // vue 2
      }
    };
    document.body.addEventListener("click", el.clickOutsideEvent);
  },
  beforeUnmount: (el) => {
    document.body.removeEventListener("click", el.clickOutsideEvent);
  },
};

const keyCode = {
  LEFT: 37,
  UP: 38,
  RIGHT: 39,
  DOWN: 40,
  ENTER: 13,
  noIndex: -1
}
export default {

  
  name: "MCombobox",
  directives:{
    clickoutside,
  },
  created() {

    this.loadDepartment();
    //render data khi ấn nút sửa
    this.dataSelected = this.valueRender;

  },
  beforeUpdate() {

  },

  watch: {
    //ban đầu dataList bị ẩn nên data chưa được render ra
    //mà chức năng search sẽ ảnh hưởng đến việc data hiển thị trong lần đầu
    showDataList() {
      this.items.forEach((item, index) => {
        this.isSearch[index] = true;
      })
    },



  },
  methods: {

    /**
     * Ẩn danh sách item
     * NVMANH (31/07/2022)
     */
     hideListData() {
      this.showDataList = false;
    },

    /**
     * focus vào input comboboxs
     * author: HMHieu(10/9/2022)
     */
    comboboxFocus(){
        this.$emit('validateDepartment');
      },
    /**
     * Hàm chọn dữ liệu
     * author: HMHieu(10/9/2022)
     */
    selectedDataCombobox(item, index) {
      this.selected = index;
      this.dataSelected = item[this.text];
      this.showDataList = false;
      //gửi đối tượng được chọn
      this.$emit('objectItemCombobox', item);

    },
    loadDepartment() {

      try {
            if(!common.checkEmptyData(sessionStorage.getItem('departments'))){
            this.items=JSON.parse(sessionStorage.getItem('departments'));
            }
            else{//chưa có dữ liệu trong localstorage
                fetch(this.url)
                .then(res => res.json())
                .then(res => {
                    this.items = res;              
                    sessionStorage.setItem('departments', JSON.stringify(this.items));
                    // this.showDataList =! this.showDataList;
                })  
            }
        } catch (error) {
          console.log(error);
        }




      // fetch(this.url)
      //   .then(res => res.json())
      //   .then(res => {
      //     this.items = res;
      //   })
    },
    /**
     * Hàm điều hướng lên xuống
     * author: HMHieu(10/9/2022)
     */
    directItemDownUp(event) {
      if (this.selected == null) { this.selected = keyCode.noIndex; }
      let key = event.keyCode;
      let selectedCurrent = this.selected;


      switch (key) {
        case keyCode.DOWN:
          // this.$refs[`focus_${this.selectedCurrent + 1}`].click();
          if (selectedCurrent < this.items.length - 1) {
            selectedCurrent++;
            while (!this.isSearch[selectedCurrent]) { //nếu bị ẩn
              selectedCurrent++;
              if (selectedCurrent == this.items.length) {
                selectedCurrent = this.selected;
                break;
              }
            }
          }
          this.selected = selectedCurrent;
          break;
        case keyCode.UP:
          if (selectedCurrent > 0) {
            selectedCurrent--;
            while (!this.isSearch[selectedCurrent]) { //nếu bị ẩn
              selectedCurrent--;
              if (selectedCurrent == -1) {
                selectedCurrent = this.selected;
                break;
              }
            }
          }
          this.selected = selectedCurrent;
          break;
        case keyCode.ENTER:
          this.dataSelected = this.items[this.selected][this.text];
          this.$emit('objectItemCombobox', this.items[this.selected]);
          this.showDataList = false;
          //reset lại
          this.selected = keyCode.noIndex;
          break;
        default:
          break;
      }
    },
    openDataCombobox(event) {
      try {
        let key = event.keyCode;
        switch (key) {
          case keyCode.DOWN:
            this.showDataList = true;
            break;
        }
      }
      catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm tìm kiếm
     * author: HMHieu(10/9/2022)
     */
    search() {
      this.showDataList = true;
      if (this.dataSelected != '' || this.dataSelected == undefined) {
        let textInput = processData(this.dataSelected);
        this.items.forEach((item, index) => {
          this.isSearch[index] = false;
          let temp = processData(item[this.text]);
          let code = processData(item[this.code]);
          // console.log(this.$refs[`show_${index}`]);
          // console.log(item);
          if (temp.search(textInput) != keyCode.noIndex) { // nếu có chứa chuỗi
            this.isSearch[index] = true;
            this.directItemDownUp;           
          }
          if (code.search(textInput) != keyCode.noIndex) {
            this.isSearch[index] = true;
            this.directItemDownUp;
          }
        })
      } else { //nếu rỗng thì hiển thị tất cả
        this.items.forEach((item, index) => {
          this.isSearch[index] = true;
          this.directItemDownUp;
          // if(keyCode.ENTER){
          //   this.$emit("objectItemCombobox", this.items[this.selected]);
          //   this.isSearch[index] = false;
          // }

        })
        this.selected = null;
        //gửi về 1 object rỗng
        
        
        this.$emit("objectItemCombobox", {});
      }

    },

  },
  props: {
    url: String,
    code: String,
    text: String,
    valueRender: String,//ID render lên combobox
    emptyCombobox: Boolean
  },
  data() {
    return {
      items: [],
      showDataList: false,
      selected: null, // làm nổi bật cái được chọn
      dataSelected: '', //text: dữ liệu được chọn
      textSearch: '',
      isSearch: [],
    }
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
@import url(../../css/base/combobox.css);
</style>
