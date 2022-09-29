// import { DocSearchModal, useDocSearchKeyboardEvents } from "@docsearch/react";
// import { useState, useCallback, useRef } from "preact/hooks";
// import { createPortal } from "preact/compat";
// import { ALGOLIA } from "../../config";
// import "@docsearch/css";
// import "./Search.scss";

// export default function Search() {
//     const [isOpen, setIsOpen] = useState(false);
//     const searchButtonRef = useRef<HTMLButtonElement>(null);
//     const [initialQuery, setInitialQuery] = useState("");

//     const onOpen = useCallback(() => {
//         setIsOpen(true);
//     }, [setIsOpen]);

//     const onClose = useCallback(() => {
//         setIsOpen(false);
//     }, [setIsOpen]);

//     const onInput = useCallback(
//         (e: any) => {
//             setIsOpen(true);
//             setInitialQuery(e.key);
//         },
//         [setIsOpen, setInitialQuery]
//     );

//     useDocSearchKeyboardEvents({
//         isOpen,
//         onOpen,
//         onClose,
//         onInput,
//         searchButtonRef
//     });

//     return (
//         <>
//             <button type="button" ref={searchButtonRef} onClick={onOpen} className="search-input">
//                 <svg width="24" height="24" fill="none">
//                     <path
//                         d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
//                         stroke="currentColor"
//                         strokeWidth="2"
//                         strokeLinecap="round"
//                         strokeLinejoin="round"
//                     />
//                 </svg>

//                 <span>Search</span>

//                 <span className="search-hint">
//                     <span className="sr-only">Press </span>

//                     <kbd>/</kbd>

//                     <span className="sr-only"> to search</span>
//                 </span>
//             </button>

//             {isOpen &&
//                 createPortal(
//                     <DocSearchModal
//                         initialQuery={initialQuery}
//                         initialScrollY={window.scrollY}
//                         onClose={onClose}
//                         indexName={ALGOLIA.indexName}
//                         appId={ALGOLIA.appId}
//                         apiKey={ALGOLIA.apiKey}
//                         transformItems={(items) => {
//                             return items.map((item) => {
//                                 // We transform the absolute URL into a relative URL to
//                                 // work better on localhost, preview URLS.
//                                 const a = document.createElement("a");
//                                 a.href = item.url;
//                                 const hash = a.hash === "#overview" ? "" : a.hash;
//                                 return {
//                                     ...item,
//                                     url: `${a.pathname}${hash}`
//                                 };
//                             });
//                         }}
//                     />,
//                     document.body
//                 )}
//         </>
//     );
// }

// TODO: uncomment line above when algolia docsearch support ESM

import "./Search.scss";

export default function Search() {
    return (
        <>
            <button type="button" className="search-input">
                <svg width="24" height="24" fill="none">
                    <path
                        d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                        stroke="currentColor"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                    />
                </svg>

                <span>Search</span>

                <span className="search-hint">
                    <span className="sr-only">Press </span>

                    <kbd>/</kbd>

                    <span className="sr-only"> to search</span>
                </span>
            </button>
        </>
    );
}
