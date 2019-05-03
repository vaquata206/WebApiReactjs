import React from 'react';
import Tree from "./Tree";
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { Spinner } from 'react-bootstrap';


class FeatureTree extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            features: []
        };

        this.loadDataFeature = this.loadDataFeature.bind(this);
    }

    componentWillMount() {
        this.loadDataFeature(this.props.appId);
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.appId !== nextProps.appId) {
            this.loadDataFeature(nextProps.appId);
        }
    }

    loadDataFeature(appId) {
        this.setState({ loading: true });
        axios.get(ApiPaths.GetFeatureNodes + "?id=" + (appId || 0)).then(response => {
            let list = response.data;
            (list || []).forEach(value => {
                value.icon = "treenode-folder";
            });
            this.setState({ features: list, loading: false });
        });
    }

    render() {
        const { loading, features } = this.state;
        if (loading) {
            return <Spinner animation="border" size="sm" />;
        } else {
            return <Tree nodes={features} onChange={this.props.onChange} ref={c => { this.tree = c; }} />;
        }
    }
}

export default FeatureTree;