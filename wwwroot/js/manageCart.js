let orderItems = JSON.parse(localStorage.getItem("order")) ?? [];
const setCartTotal = () => $('.cart-total').html('$' + orderItems.reduce((acc, item) => acc + item.price * item.quantity, 0));
const toggleCartButtons = show => {
    if (!show) {
        $(".checkout-cart").hide();
        $(".clear-cart").hide();
    } else if (show) {
        $(".checkout-cart").show();
        $(".clear-cart").show();
    }
    else return;
};

$(document).ready(function () {
    setCartTotal(orderItems);

    if (orderItems.length == 0)
        toggleCartButtons(0);

    $('.no-cart-items').html(orderItems.length);
    for (let item of orderItems)
        $('#cart-dropdown-menu').append(createCartEntry(item));

    $(document).on('click', '.remove-from-cart', function () {
        let hatId = $(this).data('hat-id');
        orderItems = orderItems.filter(el => el.id != $(this).data('hat-id'));
        
        $('.order-total').html("$" + orderItems.reduce((acc, item) => acc + item.price * item.quantity, 0));
        $('.no-cart-items').html(orderItems.length);

        if (orderItems.length == 0)
            toggleCartButtons(0);

        setCartTotal();
        localStorage.setItem("order", JSON.stringify(orderItems));
        $('li#' + hatId).remove();
    });

    $(document).on('click', '.add-to-cart', addToCart);
    $(document).on('click', '.buy-now', function() {
        if (addToCart.apply(this) != 0)
            window.location.href = "/Orders/Create";
    });

    $(document).on('click', '.clear-cart', clearCart);
});