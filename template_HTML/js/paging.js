$(document).ready(function(){
    $(document).on('click', '#icon__paging', btnSelectPagingOnClick);
    $(document).on('click', '#icon__paging--up', btnSelectPagingClose);
    $(document).on('click', '.drop__paging .drop__paging--item ', comboboxItemOnSelect);

})
function btnSelectPagingOnClick() {          
    $("#icon__paging").hide();
    $(this.nextElementSibling).show();
    $(this.nextElementSibling.nextElementSibling).show()            
}
function btnSelectPagingClose() {          
    $("#icon__paging--up").hide();
    $(this.previousElementSibling).show();
    $(this.nextElementSibling).hide()            
}

function getValueItem(){
    var text=this.textContent;
   // const value = this.getAttribute("value");
   // console.log(value);
    // Binding text lên input:
    let parentElement = this.parentElement;
   
    let input = $(parentElement).siblings("input");
    // input.value = text;
    $(input).val(text);
    $(parentElement).hide();
    //$(upIcon).hide();
   // $(this.parentElement.previousElementSibling.previousElementSibling).show();
  
    // Lưu lại value vừa chọn:
    //$(parentElement.parentElement).data("value", value);


}