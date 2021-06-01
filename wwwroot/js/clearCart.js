function clearCart() {
    orderItems.length = 0;
    $('.cart-entry').empty();
    $('.no-cart-items').html(0);
    localStorage.setItem("order", JSON.stringify(orderItems));
    setCartTotal();
    toggleCartButtons(0);
}