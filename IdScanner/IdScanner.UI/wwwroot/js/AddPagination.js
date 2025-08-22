document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll("#franchiseTable tbody tr");
    const totalRecordsLabel = document.getElementById("totalRecords");
    const paginationContainer = document.getElementById("pagination");
    const rowsPerPageSelect = document.getElementById("rowsPerPage");

    let currentPage = 1;
    let rowsPerPage = parseInt(rowsPerPageSelect.value);

    function renderTable() {
        let start = (currentPage - 1) * rowsPerPage;
        let end = start + rowsPerPage;

        rows.forEach((row, index) => {
            row.style.display = (index >= start && index < end) ? "" : "none";
        });

        totalRecordsLabel.textContent = `Total Records: ${rows.length}`;
        renderPagination();
    }

    function renderPagination() {
        paginationContainer.innerHTML = "";
        let pageCount = Math.ceil(rows.length / rowsPerPage);

        // Prev button
        let prev = document.createElement("button");
        prev.textContent = "<";
        prev.disabled = currentPage === 1;
        prev.onclick = () => {
            currentPage--;
            renderTable();
        };
        paginationContainer.appendChild(prev);

        // Page buttons
        for (let i = 1; i <= pageCount; i++) {
            let btn = document.createElement("button");
            btn.textContent = i;
            btn.className = (i === currentPage) ? "active" : "";
            btn.onclick = () => {
                currentPage = i;
                renderTable();
            };
            paginationContainer.appendChild(btn);
        }

        // Next button
        let next = document.createElement("button");
        next.textContent = ">";
        next.disabled = currentPage === pageCount;
        next.onclick = () => {
            currentPage++;
            renderTable();
        };
        paginationContainer.appendChild(next);
    }
    rowsPerPageSelect.addEventListener("change", function () {
        rowsPerPage = parseInt(this.value);
        currentPage = 1;
        renderTable();
    });

    renderTable();
});