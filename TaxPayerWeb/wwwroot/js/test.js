﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
async function getData(query) {
    const url = `/home/?query=${encodeURIComponent(query.query.value)}`;
    window.location.href = url;
}
